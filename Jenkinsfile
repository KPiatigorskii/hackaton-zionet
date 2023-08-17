

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
        def testExitCode = sh(script: 'cd MssqlAccessorTests && dotnet test', returnStatus: true)

        if (testExitCode != 0) {
            currentBuild.result = 'FAILURE' // Mark the build as failed
            error "Tests failed! Exiting pipeline."
        }
        else {
            echo "All tests passed! "
        }

    }

    stage('Push Docker images to Docker Hub'){
        sh 'docker login -u kpiatigorskii -p dckr_pat_6whSoke9x4b7XCwQjpztIE3QnOg'
        //sh 'docker tag ${imageId} kpiatigorskii/hangman-app:hangman'
        sh 'docker push kpiatigorskii/competitionfront'
        sh 'docker push kpiatigorskii/mssqlaccessor'
    }


    stage('Clean up workspace') {
        sh 'rm -rf *' // Clear Jenkins pipeline folder
    }


    post {
        failure {
            stage('Notify on Test Failure') {
                echo "Tests failed! Notifying on Slack."

                // Notify on Slack
                // Add Slack notification logic here
            }
        }
    }
}