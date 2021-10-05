CREATE DATABASE SkillTest; 

use SkillTest
go

create table Employee 
(
    Employee bigint
    , Department varchar(50)
    , Jabatan varchar(50)
    , FasilitasId int
)

create table FasilitasDetail 
(
    FasilitasId bigint
    , FasilitasDesc varchar(200)
)

insert Employee(Employee, Department, Jabatan, FasilitasId)
select 1, 'IT', 'Div Head', 4 
UNION All
select 2, 'IT', 'Scrum Master', 3 
UNION All
select 3, 'IT', 'Tech Lead', 2
UNION All
select 4, 'IT', 'Software Engineer', 1
UNION All
select 5, 'IT', 'OS', 0

insert FasilitasDetail(FasilitasId, FasilitasDesc)
select 1, 'Motor, Mobil, Kapal, Pesawat' 
UNION All
select 2, 'Motor, Mobil, Kapal'
UNION All
select 3, 'Motor, Mobil'
UNION All
select 4, 'Motor'

--2a. Daftar employee, department, dan jabatan
select Employee, Department, Jabatan
from Employee 

--2b. Daftar department yang hanya memiliki < 10 employee
select Department, COUNT (*) as 'TotalEmployee'
FROM Employee 
GROUP BY Department 
HAVING COUNT(*)<10;

--2c. Daftar employee beserta informasi department, dan jabatan yang tidak memiliki fasilitas
select Employee, Department, Jabatan
from Employee
where FasilitasId = 0