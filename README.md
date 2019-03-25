# Bank Loan App

### Installation
Clone project and run Package Manager.

```sh
$ git clone https://github.com/pepillo/Bank.git
$ Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
```

### Update Connection String
Update connection string in Web.config

```
<connectionStrings>
    <add name="BankDB" 
         connectionString="Data Source=DESKTOP-9JCFSMJ\SQLEXPRESS;
         Initial Catalog=Bank;
         Integrated Security=True" 
         providerName="System.Data.SqlClient" 
    />
</connectionStrings>
```

### Reference

| Reference | Location |
| ------ | ------ |
| Database Structure | Bank/BankApp/App_Data/DB/ |
| Design Specification | Bank/BankApp/App_Data/Design_Specs/ |
