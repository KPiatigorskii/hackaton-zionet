
pipeline {
    agent any

    stages {
        stage('checkout') {
            steps {
                script {
                    git credentialsId: 'github-creds',
                    url: 'https://github.com/KPiatigorskii/hackaton-zionet.git',
                    branch: 'devops_ci'
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
                script{
                    echo "Check docker status"
                    def dockerExitCode = sh(script: 'systemctl show --property ActiveState docker | grep -c "active" ')
                    if (dockerExitCode != 1) {
                        error "Docker proccess not active"
                    } else {
                        echo "Docker proccess is active"
                    }     
                }
            }
        }

        stage('Push Docker images to Docker Hub') {
            steps {

                withCredentials([usernamePassword(credentialsId: 'docker-hub-creds', usernameVariable: 'DOCKERHUB_USERNAME', passwordVariable: 'DOCKERHUB_PASSWORD')]) {
                    echo "Pushing Docker images to Docker Hub"
                    sh "docker login -u $DOCKERHUB_USERNAME -p $DOCKERHUB_PASSWORD"
                    sh "docker push kpiatigorskii/competitionfront"
                    sh "docker push kpiatigorskii/mssqlaccessor"
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
