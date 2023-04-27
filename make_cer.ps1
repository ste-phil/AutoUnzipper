cd AutoUnzipper

openssl pkcs12 -in AutoUnzipper_TemporaryKey.pfx -clcerts -nokeys -out ../publickey.cer

cd ..