# generate-cert.ps1
param(
    [string]$CertName = "Angular Dev Cert",
    [string]$Password = "1234",
    [string]$OutputDir = ".\ssl"
)

# Create folder fo certs
if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

Write-Host "Generating self-signed cert for localhost..."

# Creating cert with SAN (localhost + 127.0.0.1)
$cert = New-SelfSignedCertificate `
    -DnsName "localhost","127.0.0.1" `
    -CertStoreLocation "cert:\CurrentUser\My" `
    -FriendlyName $CertName `
    -NotAfter (Get-Date).AddYears(1)

# Export to PFX
$secPassword = ConvertTo-SecureString -String $Password -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath "$OutputDir\server.pfx" -Password $secPassword | Out-Null

# Convert form PFX to CRT and KEY (need OpenSSL in PATH)
Write-Host "Converting PFX in CRT and KEY..."
& openssl pkcs12 -in "$OutputDir\server.pfx" -clcerts -nokeys -out "$OutputDir\server.crt" -password pass:$Password
& openssl pkcs12 -in "$OutputDir\server.pfx" -nocerts -nodes -out "$OutputDir\server.key" -password pass:$Password

Write-Host "Certificate was generated successfully:"
Write-Host "   $OutputDir\server.crt"
Write-Host "   $OutputDir\server.key"

# Add cert to trusted root
Write-Host "Adding cert into Trusted Root Certification Authorities..."
$store = New-Object System.Security.Cryptography.X509Certificates.X509Store("Root","CurrentUser")
$store.Open("ReadWrite")
$store.Add($cert)
$store.Close()

Write-Host "Certificate was added to trusted root successfully!"
