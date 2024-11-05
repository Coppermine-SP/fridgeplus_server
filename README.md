# fridgeplus_server
<img src="https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=blazor&logoColor=white"> <img src="https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white"> <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=Docker&logoColor=white"> 

**2024년 창원대학교 컴퓨터공학과 모바일프로그래밍 팀프로젝트**

### Table of Content
  - [Overview](#overview)
  - [API Documentation](#api-documentation)
  - [Configurations](#configurations)
  - [Dependencies](#dependencies)

## Overview

## API Documentation

## Configurations
**Docker Container를 통해 구동하거나, .NET 9.0 Runtime을 설치하여 직접 구동할 수 있습니다.**

환경 변수로 아래의 옵션을 구성하십시오:
|환경 변수|설명|
|-|-|
|MYSQL_CONNECTION_STRING|MySQL 서버 연결 문자열|
|AZURE_DOCUMENT_API_KEY|Azure Document Intelligence API 키|
|AZURE_DOCUMENT_API_ENDPOINT|Azure Document Intelligence API Endpoint|

## Dependencies
- **Microsoft.EntityFrameworkCore** - 9.0.0
- **Microsoft.EntityFrameworkCore.Tools** - 8.0.0
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** - 1.21.0
- **Google.Apis.Auth** - 1.68.0
- **Azure.AI.DocumentIntelligence** - 1.0.0
- **Azure.AI.OpenAI** - 2.1.0
- **MySql.EntityFrameworkCore** - 9.0.0
