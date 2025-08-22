# CI/CD Pipeline

This document describes the **CI/CD pipeline** for deploying the application to the VPS using **GitHub Actions** and **Docker Compose**.

---

## Requirements

- A **VPS** (Ubuntu 22.04 recommended).
- A non-root user (we use `www-data`) with **SSH access**.
- **Docker** and **Docker Compose** installed on the VPS.
- A GitHub repository with the application source code.
- GitHub **Secrets** configured for secure authentication.

---

## 🛠 VPS Setup

1. **Create the deployment user**:

   ```bash
   sudo adduser www-data
   sudo usermod -aG sudo www-data
   ```

2. **Add SSH key for GitHub Actions**:

   ```bash
   mkdir -p /home/www-data/.ssh
   nano /home/www-data/.ssh/authorized_keys
   chmod 700 /home/www-data/.ssh
   chmod 600 /home/www-data/.ssh/authorized_keys
   chown -R www-data:www-data /home/www-data/.ssh
   ```

   Paste your **public key** here. The matching **private key** will be stored in GitHub Secrets.

3. **Install Docker and Docker Compose**:

   ```bash
   sudo apt update
   sudo apt install -y docker.io docker-compose git
   sudo usermod -aG docker www-data
   ```

4. **Enable Docker on startup**:

   ```bash
   sudo systemctl enable docker
   sudo systemctl start docker
   ```

5. **Create project directory**:

   ```bash
   mkdir -p /home/www-data/projects/myapp
   cd /home/www-data/projects/myapp
   git clone git@github.com:your-org/your-repo.git .
   ```

---

## GitHub Secrets

In your repository, go to
**Settings → Secrets and variables → Actions**
and configure the following secrets:

- `VPS_HOST` → Your VPS IP or hostname (e.g. `123.45.67.89`)
- `VPS_USER` → Deployment user (e.g. `www-data`)
- `SSH_PRIVATE_KEY` → Private key that matches the public key in `/home/www-data/.ssh/authorized_keys`

---

## ⚙️ GitHub Actions Workflow

Create the file:
`.github/workflows/deploy.yml`

```yaml
name: Deploy to VPS

on:
  push:
    branches:
      - main # Trigger only on push to main branch

jobs:
  deploy:
    name: Deploy to VPS
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Set up SSH
        uses: webfactory/ssh-agent@v0.9.0
        with:
          ssh-private-key: ${{ secrets.SSH_PRIVATE_KEY }}

      - name: Deploy via SSH
        run: |
          ssh -o StrictHostKeyChecking=no ${{ secrets.VPS_USER }}@${{ secrets.VPS_HOST }} << 'EOF'
            set -e

            echo ">>> Navigating to project directory..."
            cd /home/www-data/projects/myapp

            echo ">>> Pulling latest changes..."
            git fetch origin main
            git reset --hard origin/main

            echo ">>> Updating Docker services..."
            docker-compose pull
            docker-compose up -d --build

            echo ">>> Cleaning unused resources..."
            docker system prune -af

            echo ">>> Deployment finished!"
          EOF
```

---

## 🔄 Workflow Execution

1. Developer pushes to **main** branch.
2. GitHub Actions is triggered.
3. The workflow connects to the VPS via SSH.
4. The latest code is pulled.
5. Docker Compose rebuilds and restarts containers.
6. Old containers and images are cleaned.

---

## Optional: Run Tests Before Deploy

You can add a **test job** before deployment:

```yaml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Run tests
        run: |
          dotnet test --no-build --verbosity normal

  deploy:
    needs: test
    runs-on: ubuntu-latest
    steps: ...
```

This ensures **deployment only happens if tests pass**.

---
