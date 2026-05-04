# Copilot Instructions

## Linee guida del progetto
- Nel progetto ricetta_dematerializzata, nella request il pinCode e il CF dell’assistito devono essere cifrati con OpenSSL/CMS usando il certificato SanitelCF-2024-2027.cer.
- Per i servizi erogatore, utilizzare le seguenti credenziali specifiche: Regione 190, AslAo 201, Codice ufficio utenza 190-201-000000, User TSTSIC00B01H501E, Password Salve123, pinCode TSTSIC00B01H501E (da cifrare).
- Nel payload InvioErogato, il campo deve essere 'prescrizioneFruita' con naming esatto (case-sensitive, minuscolo iniziale).