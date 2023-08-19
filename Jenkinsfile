
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

                        if (currentBranch.contains('competitionfront')) {
                            echo "Pushing competitionfront to Docker Hub"
                            sh "docker push kpiatigorskii/competitionfront:$currentBranch"
                        }
                        
                        if (currentBranch.contains('mssqlaccessor')) {
                            echo "Pushing mssqlaccessor to Docker Hub"
                            sh "docker push kpiatigorskii/mssqlaccessor:$currentBranch"
                        }
                    }
                }
            }
        }
    }

    post {
        always{
            echo "Clearing Jenkins pipeline folder"
            sh 'rm -rf *'
        }

        success {
            echo "Pipeline succeeded! Notifying on Slack."
            slackSend(
                color: "#00FF00",
                channel: "jenkins-notify",
                message: "${currentBuild.fullDisplayName} was succeeded",
                tokenCredentialId: 'slack-token'
            )
        }

        failure {
            echo "Pipeline failed! Notifying on Slack."
            slackSend(
                color: "#FF0000",
                channel: "jenkins-notify",
                message: "${currentBuild.fullDisplayName} was failed",
                tokenCredentialId: 'slack-token'
            )
        }
    }
}
