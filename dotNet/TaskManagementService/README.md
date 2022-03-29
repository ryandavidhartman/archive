Project Dependencies
====================

* install-package nunit TaskManagementService.Common.Tests
* install-package nunit TaskManagementService.Web.Api.Tests
* install-package moq TaskManagementService.Web.Api.Tests
* install-package nhibernate TaskManagementService.Data.SqlServer
* install-package fluentnhibernate TaskManagementService.Data.SqlServer
* install-package log4net TaskManagementService.Web.Api
* install-package nhibernate TaskManagementService.Web.Api
* install-package fluentnhibernate TaskManagementService.Web.Api
* install-package ninject TaskManagementService.Web.Api
* install-package ninject.web.common TaskManagementService.Web.Api
* install-package Microsoft.AspNet.WebApi.OData -Pre TaskManagementService.Web.Api
* install-package log4net TaskManagementService.Web.Common
* install-package nhibernate TaskManagementService.Web.Common
* install-package ninject TaskManagementService.Web.Common

nunit
------
Unit testing framework

moq
---
Mocking framework for unit testing

nhibernate / fluentnhibernate
-----------------------------
ORM (i.e. maps DB schema to POCOs)

ninject
-------
IoC dependency injection framwork

OData
-----
Open Data Protocol (a.k.a OData) is a data access protocol from Microsoft released under the Microsoft Open Specification Promise.
The protocol was designed to provide standard CRUD access to a data source via a website.

log4net
-------
Open source logging framework.  Like log4j.