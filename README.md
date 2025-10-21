<div align="center">
 <img src="/assets/images/innoclinic-logo.png" alt="InnoClinic Logo" width="" height="150"/>

[![GHA Build and Test](https://img.shields.io/github/actions/workflow/status/AlexTarski/InnoClinic/build.yml?style=flat-square&logo=github&logoColor=fff&label=Build%20and%20Test%20Workflow%20)](https://github.com/AlexTarski/InnoClinic/actions/workflows/build.yml)
[![GHA Deploy](https://img.shields.io/github/actions/workflow/status/AlexTarski/InnoClinic/deploy.yml?style=flat-square&logo=github&logoColor=fff&label=Deploy%20Workflow%20)](https://github.com/AlexTarski/InnoClinic/actions/workflows/deploy.yml)

</div>

---
InnoClinic is a microservice system for a medical center operating across multiple regional branches. It combines two Angular single‑page applications for the frontend with several .NET APIs in the backend, designed for scalability, security, and clear separation of responsibilities. The platform supports multiple data stores to fit domain needs and future growth.

---

# Technologies
## In use

[![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/en-us/languages/csharp)
[![TypeScript](https://img.shields.io/badge/typescript-%23007ACC.svg?style=for-the-badge&logo=typescript&logoColor=white)](https://www.typescriptlang.org/)
[![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E)](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
[![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white)](https://html.spec.whatwg.org/)
[![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white)](https://developer.mozilla.org/en-US/docs/Web/CSS)

[![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/)
[![Angular](https://img.shields.io/badge/angular-%23DD0031.svg?style=for-the-badge&logo=angular&logoColor=white)](https://angular.dev/)

[![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?style=for-the-badge&logo=mongodb&logoColor=white)](https://www.mongodb.com/)
[![EFCore](https://img.shields.io/badge/EFCore-purple?style=for-the-badge)](https://github.com/dotnet/efcore)
[![Amazon S3](https://img.shields.io/badge/Amazon%20S3-FF9900?style=for-the-badge&logo=amazons3&logoColor=white)](https://aws.amazon.com/s3/?nc1=h_ls)

[![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)](https://www.jwt.io/)
[![IdentityServer](https://img.shields.io/badge/IdentityServer-purple?style=for-the-badge)](https://duendesoftware.com/products/identityserver)

[![NUnit](https://img.shields.io/badge/NUnit-%2384A454?style=for-the-badge)](https://github.com/nunit)
[![Selenium](https://img.shields.io/badge/-selenium-%43B02A?style=for-the-badge&logo=selenium&logoColor=white)](https://www.selenium.dev/)

[![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)](https://github.com/AlexTarski/InnoClinic)
[![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white)](https://git-scm.com/)

[![Docker](https://img.shields.io/badge/Docker-%232496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![DockerHub](https://img.shields.io/badge/DockerHub-%232496ED?style=for-the-badge&logo=docker&logoColor=white)](https://hub.docker.com/)

[![GitHub Actions](https://img.shields.io/badge/github%20actions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white)](https://github.com/AlexTarski/InnoClinic/actions)
[![Nginx](https://img.shields.io/badge/nginx-%23009639.svg?style=for-the-badge&logo=nginx&logoColor=white)](https://nginxproxymanager.com/)
[![Serilog](https://img.shields.io/badge/Serilog-%23FF0000?style=for-the-badge)](https://serilog.net/)
[![GitHub Copilot](https://img.shields.io/badge/github_copilot-8957E5?style=for-the-badge&logo=github-copilot&logoColor=white)](https://github.com/features/copilot)

## Will use

[![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white)](https://redis.io/)
[![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![Consul](https://img.shields.io/badge/Consul-%23000000?style=for-the-badge&logo=hashicorp)](https://developer.hashicorp.com/consul)
[![Polly](https://img.shields.io/badge/Polly-%235C2983?style=for-the-badge)](https://www.pollydocs.org/)
[![Envoy](https://img.shields.io/badge/Envoy-%23AC6199?style=for-the-badge&logo=envoyproxy&logoColor=white)](https://www.envoyproxy.io/)
[![AzureFunctions](https://img.shields.io/badge/Azure_Functions-%2333CCFF?style=for-the-badge)](https://azure.microsoft.com/en-us/products/functions)

---
# Core features
- **Patients app:** A web application for clinic patients with account creation, appointment booking for specialists or services, access to medical history, and additional patient‑centric features tied to core scenarios.

- **Employees app:** A web application for administrators and doctors. Administrators manage appointments, offices, services, and user profiles. Doctors review schedules, capture appointment data, and access comprehensive patient histories and results from prior visits.

---

# Architecture
### This application based on microservice architecture

- https://microservices.io/patterns/microservices.html
- https://cloud.google.com/learn/what-is-microservices-architecture
- https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/microservices

### Application

<details>
<summary>Modules</summary>
<img src="/assets/images/schemes/Components.png" alt="InnoClinic Logo" width="1000" height=""/>
<img src="/assets/images/schemes/MessageBroker.png" alt="InnoClinic Logo" width="1000" height=""/>
</details>

<details>
<summary>Clean Architecture</summary>
Each API is designed using the clean architecture pattern:<br>
<img src="/assets/images/clean_architecture.png" alt="InnoClinic Logo" width="500" height=""/>
</details>

### DB

<details>
<summary>Schema</summary>
<img src="/assets/images/schemes/DB_Schema.jpg" alt="InnoClinic Logo" width="1000" height=""/>
</details>

# Instructions

- [Startup](https://github.com/AlexTarski/InnoClinic/wiki/Startup)
- [Logging](https://github.com/AlexTarski/InnoClinic/wiki/Logging)

# RoadMap

- [RoadMap](https://github.com/AlexTarski/InnoClinic/wiki/RoadMap)