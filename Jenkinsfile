pipeline {
    agent any

   

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = "1"
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/HungCode68/MovieBooking_MVC.git'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                // Nếu không có test project, bạn có thể comment dòng này
                bat 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish -c Release -o published'
            }
        }
    }

    post {
        success {
            echo "✅ Build & Publish thành công!"
        }
        failure {
            echo "❌ Có lỗi xảy ra trong quá trình build!"
        }
    }
}
