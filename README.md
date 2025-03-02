# TechnomediaTestTask

Схема базы данных
----

В качестве базы данных использовался LocalDB
```
SqlLocalDB.exe create "TechnomediaTestTask"
SqlLocalDB.exe start "TechnomediaTestTask"  
sqlcmd -S (localdb)\TechnomediaTestTask -Q "CREATE DATABASE TechnomediaTestTaskDB"
```
Скрипт для создания таблиц и заполнения данных [CreateTables.sql](https://github.com/Bayard1213/TechnomediaTestTask/blob/master/database/CreateTables.sql)

***connectionString*** в Web.config
```
Server=(localdb)\TechnomediaTestTask;Database=TechnomediaTestTaskDB;Trusted_Connection=True;MultipleActiveResultSets=true
```
![image](https://github.com/Bayard1213/TechnomediaTestTask/blob/master/database/database.png?raw=true)

Задачи реализации
----

 1. Распределение доступов ролей (Administrator, Director, Secretary, Worker)
 2. Реализация базового CRUD для таблиц (clients, teams, request, assignments, work_logs)
 3. Реализация дополнительной логики для отчетов (reports), заявок (requests)
 4. Покрытие методов тестами

----

### AccountController

**Задачи**: Управление учетными записями

|Метод|Доступ для роли|
|--|--|
|*Login()*: Логин пользователя, получение jwt токена для подписи.|All|
|*Register()*: Добавление нового пользователя.|Administrator|
|*GetUsers()*: Получение списка всех пользователей.|All|

### AssignmentsController

**Задачи**: Управление назначениями на выполнение заявок

|Метод|Доступ для роли|
|--|--|
|*GetAllAssignments()*: Получение списка всех назначений бригад.|Authorize|
|*GetAssignmentById(int assignmentId)*: Получение информации о конкретном назначении.|Authorize|
|*CreateAssignment(CreateAssignmentDTO newAssignmentDTO)*: Назначение заявки на бригаду.|Administrator, Secretary|
|*UpdateAssignment(int assignmentId, UpdateAssignmentDTO updatedAssignmentDTO)*: Обновление информации о назначении.|Administrator, Secretary|
|*DeleteAssignment(int assignmentId)*: Удаление назначения.|Administrator, Secretary|
|*GetAssignmentsByTeamId (int teamId)*: Получение назначений для конкретной бригады.|Administrator, Worker|

### ClientsController

**Задачи**: Управление клиентами

|Метод|Доступ для роли|
|--|--|
|*GetAllClients()*: Получение списка всех клиентов.|Authorize|
|*GetClientById(int clientId)*: Получение информации о конкретном клиенте.|Authorize|
|*CreateClient(CreateClientDTO newClientDTO)*: Регистрация нового клиента.|Secretary, Administrator|
|*UpdateClient(int clientId, UpdateClientDTO updatedClientDTO)*: Обновление информации о существующем клиенте.|Secretary, Administrator|
|*DeleteClient(int clientId)*: Удаление клиента.|Secretary, Administrator|

### RequestsController

**Задачи**: Управление заявками

|Метод|Доступ для роли|
|--|--|
|*GetAllRequest(int requestId)*: Получение списка всех заявок.|Authorize|
|*GetRequestById(int requestId)*: Получение информации о конкретной заявке.|Authorize|
|*CreateRequest(CreateRequestDTO newRequestDTO)*: Регистрация новой заявки.|Administrator, Secretary|
|*UpdateRequest(int requestId, UpdateRequestDTO updatedRequestDTO)*: Обновление информации о заявке.|Administrator, Secretary|
|*DeleteRequest(int requestId)*: Удаление заявки.|Administrator, Secretary|
|*ChangeStatusRequestOnCompleted(int requestId, CompleteStatusRequestDTO completeStatusRequestDTO)*: Обновление информации о заявке, при завершении выполнения работ.|Administrator, Worker|

### TeamsController

**Задачи**: Управление бригадами

|Метод|Доступ для роли|
|--|--|
|*GetAllTeams()*: Получение списка всех бригад.|Authorize|
|*GetTeamById(int teamId)*: Получение информации о конкретной бригаде.|Authorize|
|*CreateTeam(CreateTeamDTO newTeamDTO)*: Регистрация новой бригады.|Administrator, Director|
|*UpdateTeam(int teamId, UpdateTeamDTO updatedTeamDTO)*: Обновление информации о бригаде.|Administrator, Director|
|*DeleteTeam(int teamId)*: Удаление бригады.|Administrator,Director|

### WorkLogsController

**Задачи**: Управление журналами работ

|Метод|Доступ для роли|
|--|--|
|*GetAllWorkLogs()*: Получение списка всех журналов работы.|Authorize|
|*GetWorkLogById(int workLogId)*: Получение информации о конкретном журнале работ.|Authorize|
|*CreateWorkLog(CreateWorkLogDTO newWorkLogDTO)*: Добавление новой записи о работе.|Administrator, Worker|
|*UpdateWorkLog(int workLogId, UpdateWorkLogDTO updatedWorkLogDTO)*: Обновление информации о записи о работе.|Administrator, Worker|
|*DeleteWorkLog(int workLogId)*: Удаление записи о работе.|Administrator, Worker|
|*GetWorkLogsByTeamId(int teamId)*: Получение всех журналов работ по конкретной бригаде.|Administrator, Worker|
|*GetWorkLogsByRequestId(int requestId)*: Получение всех журналов работ по конкретной заявке.|Administrator, Worker|

### ReportsController 

**Задачи**: Предоставление отчетов

|Метод|Доступ для роли|
|--|--|
|*GetMonthlyReport(Month month, int year)*: Получение отчета по работе бригад за месяц. Включает количество выполненных заказов каждой бригадой и общее время, затраченное на выполнение заявок.|Director, Administrator|
|*GetMonthlyReportByTeam(Month month, int year, int teamId)*: Получение отчета по работе конкретной бригады за месяц. Включает количество выполненных заказов конкретной бригады и общее время, затраченное на выполнение заявок.|Director, Administrator|



