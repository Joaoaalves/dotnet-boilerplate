Perfect, Cláudin 🙌
Here’s a clean **Markdown guide in English** that you can drop directly into your official documentation.
I’ve structured it with sections, code blocks, and hints. At the end, I added a placeholder for the **CI/CD with GitHub Actions** so you can extend it later.

---

````markdown
# Deploy VPS Ubuntu

This document explains how to set up a fresh VPS from scratch, configure Docker + Docker Compose, secure the server, and deploy your application.  
Later, we will extend this guide with **CI/CD via GitHub Actions** to automate deployments on every push to `main`.

---

## 1. Connect to VPS

Log into the VPS using the root account:

```bash
ssh root@YOUR_SERVER_IP
```
````

---

## 2. Create a Non-Root User

Create a new user (e.g. `www-data`):

```bash
adduser www-data
```

Grant sudo privileges:

```bash
usermod -aG sudo www-data
```

---

## 3. Configure SSH Access

Copy your SSH keys to the new user:

```bash
rsync --archive --chown=www-data:www-data ~/.ssh /home/www-data
```

Test login:

```bash
ssh www-data@YOUR_SERVER_IP
```

---

## 4. Install System Packages

Update and install basic tools:

```bash
sudo apt update && sudo apt upgrade -y
sudo apt install -y curl git ufw htop unzip
```

---

## 5. Install Docker

Install Docker via the official script:

```bash
curl -fsSL https://get.docker.com | sh
```

Add the new user to the Docker group:

```bash
sudo usermod -aG docker www-data
```

> **Note:** You must log out and log back in for group changes to take effect.

Verify installation:

```bash
docker ps
```

---

## 6. Install Docker Compose

Download Docker Compose binary:

```bash
sudo curl -L "https://github.com/docker/compose/releases/download/v2.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
```

Apply execution permissions:

```bash
sudo chmod +x /usr/local/bin/docker-compose
```

Verify:

```bash
docker-compose --version
```

---

## 7. Enable Docker on Boot

Enable Docker service at startup:

```bash
sudo systemctl enable docker
sudo systemctl start docker
```

Check status:

```bash
systemctl status docker
```

---

## 8. Configure Firewall

Allow only required ports (SSH, HTTP, HTTPS):

```bash
sudo ufw allow OpenSSH
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw enable
```

Check firewall status:

```bash
sudo ufw status
```

---

## 9. Prepare Project Directory

Inside the `www-data` home directory:

```bash
cd /home/www-data
mkdir -p projects/myapp
cd projects/myapp
```

Place your `docker-compose.yml` here.

---

## 10. Deploy with Docker Compose

Start services:

```bash
docker-compose up -d
```

Verify running containers:

```bash
docker ps
```

---

## 11. Auto-Restart Policy

In `docker-compose.yml`, make sure each service has a restart policy:

```yaml
version: "3.9"
services:
  nginx:
    image: nginx:latest
    restart: always
    ports:
      - "80:80"
      - "443:443"
```

This ensures services automatically restart on VPS reboot.

---

## 12. Logs and Monitoring

View logs of a container:

```bash
docker logs -f <container_name>
```

---

## 13. Next Step: CI/CD with GitHub Actions

In the next step, we will configure **GitHub Actions** so that every push to `main` will:

1. SSH into the VPS
2. Pull the latest code
3. Run `docker-compose up -d --build` to redeploy the application

This will ensure **zero-downtime automatic deployments**.

---

```

---

Would you like me to go ahead and **draft the full GitHub Actions workflow file** (`.github/workflows/deploy.yml`) that connects to your VPS via SSH and redeploys automatically?
```
