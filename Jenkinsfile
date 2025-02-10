pipeline {
    agnet ang
    environment {
        GIT_REPO = 'https://github.com/Eco-PowerHub/Back-end.git'
        BRANCH = 'Dev'
        IMAGE_NAME = 'khaledmahmoud7/ecopower-asp'
        IMAGE_TAG = "${BUILD_NUMBER}"
        DOCKERHUB_CREDENTIALS = credentials('dockerhub')
    }

    stages {

        stage("Fetch Git Repo") {
            stepes {
                git branch: "${BRANCH}", url: "${GIT_REPO}"
            }    
        }

        stage("Restore Dependencies") {
            stepes {
                sh 'dotnet restore'
            }
        }

        // stage("Building") {
        //     stepes {
        //         sh 'dotnet build --no-restore -c Release'
        //     }
        // }

        stage("Publish") {
            stepes {
                sh 'RUN dotnet publish -c Release -o publish'
            }
        }

        stage('Run Unit Tests') {
            steps {
                sh 'dotnet test --no-build --verbosity normal'
            }
        }
    }

    post {
        success {
            echo 'Build and Tests completed successfully!'
        }
        failure {
            echo 'Build or Tests failed. Check the logs for details.'
        }
    }
}