

// 1) checkout code
// 2) run dotnet test
// 3.1) if tests ok - push to regisltry, notify on slack, create PR to default dev branch
// 3.2) if tests failed, notify on slack
// 4) clear jenkins pipeline folder



node{
    stage('checkout') {
            git \
                credentialsId: 'github-creds', \
                url: 'https://github.com/KPiatigorskii/hackaton-zionet.git', \
                branch: 'devops_ci'
        }

    stage('test solution') {
        sh 'pwd'
        sh 'ls -al'
        sh 'cd MssqlAccessorTests && dotnet test'

    }

    stage('Push Docker images to Docker Hub'){
        sh 'docker login -u kpiatigorskii -p dckr_pat_6whSoke9x4b7XCwQjpztIE3QnOg'
        //sh 'docker tag ${imageId} kpiatigorskii/hangman-app:hangman'
        sh 'docker push kpiatigorskii/competitionfront'
        sh 'docker push kpiatigorskii/mssqlaccessor'
    }

    stage('Check test results and notify') {
            def testResults = currentBuild.rawBuild.getAction(hudson.tasks.junit.TestResultAction.class)
            
            if (testResults && testResults.result) {
                def passedTests = testResults.result.passedTests
                def failedTests = testResults.result.failedTests

                if (failedTests == 0) {
                    // Tests passed
                    echo "All tests passed! Pushing to registry, notifying on Slack, and creating PR."

                    // Push to registry
                    sh 'git checkout main' // Make sure we are on the main branch
                    sh 'git pull origin main' // Pull the latest changes
                    // Perform necessary actions for pushing and creating PR

                    // Notify on Slack
                    // Add Slack notification logic here

                    // Create PR to default dev branch
                    // Add PR creation logic here
                } else {
                    // Tests failed
                    echo "Tests failed! Notifying on Slack."

                    // Notify on Slack
                    // Add Slack notification logic here
                }
            } else {
                error "Test results not found!"
            }
        }

        stage('Clean up workspace') {
            sh 'rm -rf *' // Clear Jenkins pipeline folder
        }


    // stage('Deploy'){
    //         sshagent(['my-creds']) {
    //             // ssh -o StrictHostKeyChecking=no ubuntu@${ec2_instanse} "rm -rf /home/ubuntu/hangman"
    //             // scp -o StrictHostKeyChecking=no -r ${WORKSPACE}  ubuntu@${ec2_instanse}:/home/ubuntu/hangman/
    //             sh """
    //             echo "${WORKSPACE}"
    //             ls -l

    //             ssh -o StrictHostKeyChecking=no ubuntu@${ec2_instanse} 'sudo docker login -u kpiatigorskii -p dckr_pat_6whSoke9x4b7XCwQjpztIE3QnOg'
    //             ssh -o StrictHostKeyChecking=no ubuntu@${ec2_instanse} 'sudo docker pull kpiatigorskii/hangman-app:hangman'
    //             ssh -o StrictHostKeyChecking=no ubuntu@${ec2_instanse} 'sudo docker run -d kpiatigorskii/hangman-app:hangman'
    //             """
    //             }
    // }
}