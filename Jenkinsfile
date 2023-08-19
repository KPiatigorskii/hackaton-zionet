
pipeline {
    agent any
    parameters {
        gitParameter branchFilter: 'origin/(.*)', defaultValue: 'main', name: 'BRANCH', type: 'PT_BRANCH'
    }
    stages {
        stage('checkout') {
            steps {
                script {
                    echo "$params.BRANCH"
                    git credentialsId: 'github-creds',
                    url: 'https://github.com/KPiatigorskii/hackaton-zionet.git',
                    branch: "$params.BRANCH"
                }
            }
        }

        stage('Test solution') {
            steps {
                script {
                    echo "Testing solution..."
                    def testExitCode = sh(script: 'cd MssqlAccessorTests && dotnet test', returnStatus: true)

                    if (testExitCode != 0) {
                        error "Tests failed! Exiting pipeline."
                    } else {
                        echo "All tests passed!"
                    }
                }
            }
        }

        stage('Push Docker images to Docker Hub') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'docker-hub-creds', 
                                    usernameVariable: 'DOCKERHUB_USERNAME', 
                                    passwordVariable: 'DOCKERHUB_PASSWORD')]) {
                        echo "Pushing Docker images to Docker Hub"
                        sh "docker login -u $DOCKERHUB_USERNAME -p $DOCKERHUB_PASSWORD"
                        
                        def currentBranch = params.BRANCH
                        echo "currentBranch: $params.BRANCH"
                        def trimmedBranchName = currentBranch.replaceAll('^competitionfront_|^mssqlaccessor_', '')

                        if (currentBranch.contains('competitionfront')) {
                            sh " docker build -t kpiatigorskii/competitionfront:$trimmedBranchName . -f CompetitionFront/Dockerfile"
                            echo "Pushing competitionfront to Docker Hub"
                            sh "docker push kpiatigorskii/competitionfront:$trimmedBranchName"
                        }
                        
                        if (currentBranch.contains('mssqlaccessor')) {
                            sh "docker build -t kpiatigorskii/mssqlaccessor:$trimmedBranchName . -f CompetitionBack/Dockerfile"
                            echo "Pushing mssqlaccessor to Docker Hub"
                            sh "docker push kpiatigorskii/mssqlaccessor:$trimmedBranchName"
                        }
                    }
                }
            }
        }
    }

post {
        always {
            echo "Clearing Jenkins pipeline folder"
            sh 'rm -rf *'
        }

        success {
            echo "Pipeline succeeded! Notifying on Slack."
            def buildUrl = env.BUILD_URL
            slackSend(
                color: "#00FF00",
                channel: "jenkins-notify",
                message: "<${buildUrl}|${currentBuild.fullDisplayName}> was succeeded",
                tokenCredentialId: 'slack-token'
            )
        }

        failure {
            echo "Pipeline failed! Notifying on Slack."
            def buildUrl = env.BUILD_URL
            slackSend(
                color: "#FF0000",
                channel: "jenkins-notify",
                message: "<${buildUrl}|${currentBuild.fullDisplayName}> was failed",
                tokenCredentialId: 'slack-token'
            )
        }
    }
}
