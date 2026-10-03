# WareHouseApp

WareHouseApp is a Windows Forms warehouse-management coursework application built with C# on .NET Framework 4.7.2. It uses SQL Server LocalDB and `WarehouseDB.mdf` for account, inventory, customer, and employee data.

## Features

- Employee sign-up creates a `Person` account and linked `Employee` row in one transaction.
- Login checks credentials against `Person`; the dashboard opens only after successful authentication. Logging out returns to the login screen.
- Inventory management supports viewing, adding, editing, and deleting materials, plus receiving and shipping quantities. Stock cannot be shipped below zero.
- Customer management supports viewing and full CRUD.
- Employee management supports viewing and full CRUD. Deleting the last employee linked to a Person also removes that Person account.
- Database and validation failures are presented to users through WinForms message boxes.

## Design and OOP

- `Person` is an abstract base class that encapsulates identity and session state and defines the login, password-change, and logout contract.
- `Admin`, `Employee`, and `ShippingOperator` inherit from `Person` and provide role behavior. The dashboard accepts the authenticated user as a `Person` and calls `Logout` polymorphically.
- `Customer`, `Employee`, and `Material` expose model state through properties. Database access is kept in the People/Material models and `AccountService`; forms handle input, display, and user feedback.
- `AccountService.RegisterEmployee` allocates the non-identity `Person.EmpId` while holding a serializable transaction, then inserts both linked rows. A failed insert rolls the transaction back.

## Database and setup

1. Open `WareHouseApp.sln` in Visual Studio with the .NET Framework 4.7.2 targeting pack and SQL Server LocalDB installed.
2. Ensure `WareHouseApp\WarehouseDB.mdf` is present. The project copies it to the output directory, and `App.config` connects using `|DataDirectory|`.
3. Build and run the `WareHouseApp` project. Sign up an employee account, log in, and use the dashboard screens.

The attached MDF was inspected during implementation. Its live schema differs from some earlier schema notes:

| Table | Live column details |
|---|---|
| `Person` | `EmpId int` primary key (not identity); `Username nchar(10)`; `Password nchar(10)` |
| `Material` | `Id int` (not identity); `Name nvarchar(50)`; `Count int` |
| `Customer` | `CustomerId int` identity; `Name nvarchar(50)`; `Email nvarchar(50)`; `Phone nvarchar(20)` |
| `Employee` | `EmployeeId int` identity; `Name nvarchar(50)`; `Position nvarchar(50)`; `Salary decimal(18,2)`; `EmpId int` references `Person.EmpId` |

The application validates these limits and allocates `Person.EmpId` and `Material.Id` transactionally because they are not identity columns in the attached database. `Email`, `Phone`, `Position`, `Salary`, `Count`, and `Employee.EmpId` are nullable in the inspected database, although the application requires values where needed by its forms. If another MDF has different definitions, verify its schema before running the app.

**Credential limitation:** `Person.Password` is `nchar(10)` and the current database stores it as plaintext to remain compatible with the supplied database. This is a coursework-compatible limitation, not an appropriate production password-storage design. A production deployment should migrate the column and use a salted password hash.

## Exception handling and database safety

- SQL commands use parameters; user-entered values are not concatenated into SQL statements.
- Data-access operations catch failures, add operation context, and close/dispose connections in `finally` blocks (with command/adapter disposal through `using`).
- Account registration and inventory ID allocation use serializable transactions to avoid partial related rows and conflicting IDs.
- The UI validates required values and database column limits before saving, and displays operation failures rather than reporting false success.

## Manual acceptance checklist

- Create an employee account; confirm a duplicate username is rejected and the new employee appears in Employee Management.
- Log in with the new credentials; confirm invalid credentials do not open the dashboard. Log out and confirm the login screen returns.
- Add, select, update, and delete a material. Receive stock, ship part of the quantity, and verify an attempted shipment exceeding stock is rejected.
- Add, select, update, and delete customers and employees; verify each change remains after refreshing the form.
- Test invalid and over-length values and confirm the app presents a clear validation/database message.

## Implementation and version control notes

Implementation was performed in feature-sized changes: inspect the MDF and existing forms, add shared database configuration, implement transactional account registration/authentication, wire login/sign-up/logout, complete inventory CRUD and stock movements, then align the person/customer/employee models and document the system. Builds were run after the login and inventory work; perform another full build and the acceptance checks above before submission.

The workspace is a Git repository on branch `not-master`, with remote `origin` pointing to the project repository. The inspected working tree contains local modifications, deleted files, and untracked database/build artifacts; no claim is made that this implementation has been committed. For version control, review `git status` before each commit, keep feature changes in small commits (for example, `Add account registration` and `Complete inventory CRUD`), use a topic branch for substantial work, and push the reviewed branch to `origin`. Keep generated `.vs`, `bin`, and `obj` files out of commits and preserve database snapshots/backups separately from source changes.
