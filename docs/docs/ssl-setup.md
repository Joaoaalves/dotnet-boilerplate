# SSL Setup with Certbot

This section explains how to secure your domains with **Let's Encrypt SSL certificates** using **Certbot**.
We will use the **DNS challenge** method, which allows SSL generation even when Nginx is already running inside Docker.

## 1. Run Certbot for Your API Domain

Run the following command to generate a certificate for your API domain:

```bash
docker run -it --rm \
  -p 8080:80 \
  -v /etc/letsencrypt:/etc/letsencrypt \
  -v /var/lib/letsencrypt:/var/lib/letsencrypt \
  certbot/certbot certonly \
  -d api.{YOUR_DOMAIN} \
  --manual --preferred-challenges dns
```

- `-d api.{YOUR_DOMAIN}` → replace with your API domain.
- You will be prompted to add a **DNS TXT record**. Do this in your domain registrar’s DNS panel.
- Once propagation is complete, Certbot will validate and issue the certificate.

The certificates will be stored under:

```
/etc/letsencrypt/live/api.{YOUR_DOMAIN}/
```

---

## 2. Run Certbot for Docs Domain

Repeat the same process for your **Docs** domain:

```bash
docker run -it --rm \
  -p 8080:80 \
  -v /etc/letsencrypt:/etc/letsencrypt \
  -v /var/lib/letsencrypt:/var/lib/letsencrypt \
  certbot/certbot certonly \
  -d docs.{YOUR_DOMAIN} \
  --manual --preferred-challenges dns
```

Certificates will be stored under:

```
/etc/letsencrypt/live/docs.{YOUR_DOMAIN}/
```

---

## 3. Run Certbot for Additional Services

If you have other subdomains (e.g., **Grafana** and **Kibana**), repeat the same command for each:

```bash
# Grafana
docker run -it --rm \
  -p 8080:80 \
  -v /etc/letsencrypt:/etc/letsencrypt \
  -v /var/lib/letsencrypt:/var/lib/letsencrypt \
  certbot/certbot certonly \
  -d grafana.{YOUR_DOMAIN} \
  --manual --preferred-challenges dns

# Kibana
docker run -it --rm \
  -p 8080:80 \
  -v /etc/letsencrypt:/etc/letsencrypt \
  -v /var/lib/letsencrypt:/var/lib/letsencrypt \
  certbot/certbot certonly \
  -d kibana.{YOUR_DOMAIN} \
  --manual --preferred-challenges dns
```

---

## 4. Verify Certificates

After issuing all certificates, verify:

```bash
ls /etc/letsencrypt/live/
```

You should see one folder per domain:

```
api.{YOUR_DOMAIN}
docs.{YOUR_DOMAIN}
grafana.{YOUR_DOMAIN}
kibana.{YOUR_DOMAIN}
```

---

## 5. Renew Certificates

Let's Encrypt certificates expire every **90 days**.
To renew all at once, run:

```bash
docker run -it --rm \
  -p 8080:80 \
  -v /etc/letsencrypt:/etc/letsencrypt \
  -v /var/lib/letsencrypt:/var/lib/letsencrypt \
  certbot/certbot renew
```

> **Tip:** You can automate this with a cron job to ensure your certificates never expire.
