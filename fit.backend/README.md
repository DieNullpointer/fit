# Backend für die FIT Webapp


### appsettings.Development.json

Lege im Verzeichnis *FitManager.Webapi* die Datei *appsettings.Development.json* an.

- Generiere ein Secret auf https://generate.plus/en/base64 mit 128 Bytes Länge und schreibe es in *JwtSecret*.
- Schreibe deinen AD User in *Searchuser* und *Searchpass*. Die Datei *appsettings.Development.json*
  wird deswegen nicht in das Repository übertragen.
- Lokale Admins können *mit , getrennt* (kein Array) hinterlegt werden. Diese Account bekommen die
  Rolle *Management* im JWT.

```javascript
{
  "ConnectionStrings": {
    "SqlServer": "Server=127.0.0.1,1433;Initial Catalog=FitDb;User Id=sa;Password=SqlServer2019"
  },
  "Searchuser": "",
  "Searchpass": "",
  "JwtSecret": "",
  "LocalAdmins": "vogl",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Controller für die AD Authentifizierung

**POST /api/user/login** mit `{"username": "myUser", "password": "myPassword"}` liefert ein JSON
Object mit der User ID und dem Token zurück. Wurde ein Suchuser hinterlegt, wird im Development
Mode das Passwort nicht validiert und es werden die Daten des anderen Users zurückgegeben.

**GET /api/user/me** braucht im Header den Bearer Token. Die Route liefert die gespeicherten
Daten im Token zurück (Rolle, Klasse, ...)
