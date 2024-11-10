# fridgeplus_server
<img src="https://img.shields.io/badge/ASP.NET-512BD4?style=for-the-badge&logo=blazor&logoColor=white"> <img src="https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white"> <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=Docker&logoColor=white"> 

**2024년 창원대학교 컴퓨터공학과 모바일프로그래밍 팀프로젝트**

### Table of Content
  - [Overview](#overview)
  - [API Documentation](#api-documentation)
  - [Configurations](#configurations)
  - [Dependencies](#dependencies)

## Overview
- Google JWT를 통한 사용자 인증
- Azure Document Intelligence + OpenAI Service를 통한 영수증 인식 및 분류
- Code-First Approach를 통한 데이터베이스 구축

## API Documentation
### Table of Content
- **IntelligenceController**
  
    - [영수증에서 불러오기](#영수증에서-불러오기)
    - [요리 추천하기](#요리-추천하기)
- **FridgeController**

    - [카테고리 가져오기](#카테고리-가져오기)
    - [아이템 가져오기](#아이템-가져오기)
    - [아이템 추가하기](#아이템-추가하기)
    - [아이템 삭제하기](#아이템-삭제하기)
- **AuthenticationController**
  
    - [사용자 인증하기](#사용자-인증하기)
    - [사용자 정보 가져오기](#사용자-정보-가져오기)
    - [로그아웃 하기](#로그아웃-하기)
      
- - -

### 영수증에서 불러오기
영수증 이미지에서 식품 개체를 반환합니다.
|Method|URL|인증|
|-|-|-|
|POST|api/intelligence/importFromReceipt|true|

#### 요청
**form-data**
|Key|Type|Value|
|-|-|-|
|image|File|영수증 이미지|
```bash
curl -X POST 'https://fridgeplus.dev.cloudint.corp/api/intelligence/importFromReceipt' \
-F 'image=@"sample_receipt.jpg"'
```

#### 응답

**Json Object Array**
|Key|Type|Value|
|-|-|-|
|itemDescription|string|상품 이름|
|itemQuantity|int|상품 수량|
|categoryId|int|자동 분류된 카테고리 ID|
```json
{
    "items": [
        {
            "categoryId": 8,
            "itemDescription": "오렌지",
            "itemQuantity": 1
        },
        {
            "categoryId": 9,
            "itemDescription": "당근",
            "itemQuantity": 1
        },
        {
            "categoryId": 8,
            "itemDescription": "배",
            "itemQuantity": 1
        }
    ]
}
```
- - -
### 요리 추천하기
현재 사용자가 가진 아이템을 기반으로 할 수 있는 요리를 추천합니다.
|Method|URL|인증|
|-|-|-|
|GET|api/intelligence/insight|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/intelligence/insight'
```

#### 응답
**Json**
|이름|타입|설명|
|---|---|---|
|result|string|GPT가 현재 아이템을 기반으로 추천한 레시피|

```json
{
  "result": "당근과 감자를 이용한 간단한 요리로 감자 당근 볶음을 추천드립니다.
             감자 당근 볶음 만들기
             1. 재료 준비: 당근과 감자를 얇게 채 썰어 준비합니다.
             2. 기름에 볶기: 팬에 기름을 두르고 감자부터 넣어 중불에서 볶습니다. 감자가 어느 정도 익으면 당근을 넣고 함께 볶습니다.
             3. 간하기: 소금과 후추로 간을 맞추고, 감자가 투명해지고 당근이 부드러워질 때까지 볶습니다.
             4. 완성: 기호에 따라 파슬리나 참깨를 뿌려 마무리합니다.
             이 요리는 반찬으로도 좋고, 간단한 한 끼로도 손색이 없습니다."
}
```
- - -
### 카테고리 가져오기
카테고리 정보를 가져옵니다.
|Method|URL|인증|
|-|-|-|
|GET|api/fridge/categoryList|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/fridge/categoryList'
```

#### 응답
**Json Object Array**
|Key|Type|Value|
|-|-|-|
|categoryId|int|카테고리 ID|
|categoryName|string|카테고리 한국어 이름|
|recommendedExpirationDays|int|권장 유효기간 일수|

```json
{
    "categories": [
        {
            "categoryId": 7,
            "categoryName": "기타 신선식품",
            "recommendedExpirationDays": null
        },
        {
            "categoryId": 9,
            "categoryName": "이과류",
            "recommendedExpirationDays": 7
        },
        {
            "categoryId": 10,
            "categoryName": "육류",
            "recommendedExpirationDays": 2
        },
        {
            "categoryId": 11,
            "categoryName": "어류",
            "recommendedExpirationDays": 1
        },
        {
            "categoryId": 12,
            "categoryName": "유가공품",
            "recommendedExpirationDays": 24
        },
        {
            "categoryId": 13,
            "categoryName": "반찬",
            "recommendedExpirationDays": 7
        },
        {
            "categoryId": 47,
            "categoryName": "우유",
            "recommendedExpirationDays": 24
        },
        {
            "categoryId": 48,
            "categoryName": "계란",
            "recommendedExpirationDays": 21
        },
        {
            "categoryId": 52,
            "categoryName": "뿌리채소",
            "recommendedExpirationDays": 120
        },
        {
            "categoryId": 57,
            "categoryName": "새싹채소",
            "recommendedExpirationDays": 2
        },
        {
            "categoryId": 58,
            "categoryName": "버섯류",
            "recommendedExpirationDays": 7
        },
        {
            "categoryId": 59,
            "categoryName": "채소",
            "recommendedExpirationDays": 14
        },
        {
            "categoryId": 60,
            "categoryName": "과일",
            "recommendedExpirationDays": 7
        }
    ]
}
```
- - -

### 아이템 가져오기
현재 사용자의 아이템을 모두 가져옵니다.
|Method|URL|인증|
|-|-|-|
|GET|api/fridge/itemList|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/fridge/itemList'
```
#### 응답
**Json Object Array**
|Key|Type|Value|
|-|-|-|
|itemId|int|아이템 ID|
|categoryId|int|아이템의 카테고리 ID|
|itemOwner|string|아이템 소유자 UID|
|itemDescription|string|이름|
|itemQuantity|int|수량|
|itemImportDate|ISO-8601 Date|생성일|
|itemExpireDate|ISO-8601 Date|만료일|

```json
{
    "items": [
        {
            "itemId": 2,
            "categoryId": 9,
            "itemOwner": "101463908511110268273",
            "itemDescription": "중국산 사과",
            "itemQuantity": 1,
            "itemImportDate": "2024-11-10T00:00:00",
            "itemExpireDate": "2024-11-11T00:00:00"
        },
        {
            "itemId": 1,
            "categoryId": 9,
            "itemOwner": "101463908511110268273",
            "itemDescription": "씻은 당근",
            "itemQuantity": 2,
            "itemImportDate": "2024-11-10T00:00:00",
            "itemExpireDate": "2024-11-13T00:00:00"
        }
    ]
}
```

- - -

### 아이템 추가하기
아이템을 추가합니다.
|Method|URL|인증|
|-|-|-|
|POST|api/fridge/addItems|true|

#### 요청
**JSON Object Array**
|Key|Type|Value|
|-|-|-|
|categoryId|int|아이템의 카테고리 ID|
|itemDescription|string|이름|
|itemQuantity|int|수량|
|expires|ISO-8601 Date|만료일|

```bash
curl -X POST 'https://fridgeplus.dev.cloudint.corp/api/fridge/addItems' \
-H 'Content-Type: application/json' \
-d '{
    "items": [
        {
            "categoryId": 9,
            "itemDescription": "중국산 사과",
            "itemQuantity": 1,
            "expires": "2024-11-11T00:00:00+09:00"
        }
    ]
}'

```

#### 응답
**HTTP Status Code**

|Code|Description|
|-|-|
|200|성공|
|500|실패|

- - -

### 아이템 삭제하기
특정 아이템을 삭제합니다.
|Method|URL|인증|
|-|-|-|
|POST|api/fridge/deleteItem|true|

- - -

### 사용자 인증하기
Google JWT로 현재 세션을 인증합니다.
|Method|URL|인증|
|-|-|-|
|POST|api/auth/tokenSignIn|false|

#### 요청
**form-data**
|Key|Type|Value|
|-|-|-|
|token|Text|JWT 토큰|
```bash
curl -X POST 'https://fridgeplus.dev.cloudint.corp/api/auth/' \
-F 'token="bnBaI0ZuAP829Zq3ZiDzVO84ua5umkVgvaqr"'
```

#### 응답
**HTTP Status Code**

|Code|Description|
|-|-|
|200|인증 성공|
|400|인증 실패|

인증 실패시, 이유가 Body에 반환됨.

- - -

### 사용자 정보 가져오기
현재 세션에 로그인 된 사용자 정보를 가져옵니다.
|Method|URL|인증|
|-|-|-|
|GET|api/auth/accountInfo|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/auth/accountInfo'
```

### 응답
**Json Object**
|Key|Type|Value|
|-|-|-|
|sub|string|사용자 고유 ID|
|name|string|사용자 이름|

```json
{"sub":"101433903511515263273","name":"Coppermine"}
```

- - -

### 로그아웃 하기
현재 세션에서 로그아웃합니다.
|Method|URL|인증|
|-|-|-|
|GET|api/auth/signOut|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/auth/signOut'
```

#### 응답
**HTTP Status Code**

|Code|Description|
|-|-|
|200|로그아웃 성공|
|401|인증 실패|

## Configurations
**Docker Container를 통해 구동하거나, .NET 8.0 Runtime을 설치하여 직접 구동할 수 있습니다.**

환경 변수로 아래의 옵션을 구성하십시오:
|환경 변수|설명|
|-|-|
|MYSQL_CONNECTION_STRING|MySQL 서버 연결 문자열|
|AZURE_DOCUMENT_API_KEY|Azure Document Intelligence API 키|
|AZURE_DOCUMENT_API_ENDPOINT|Azure Document Intelligence API Endpoint|
|OPENAI_API_KEY|OpenAI API 키|
|OPENAI_API_MODEL|OpenAIGPTService에서 사용할 모델|

## Dependencies
- **Microsoft.EntityFrameworkCore** - 8.0.10 
- **Microsoft.EntityFrameworkCore.Tools** - 8.0.10
- **Microsoft.EntityFrameworkCore.Design** - 8.0.10
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** - 1.21.0
- **Google.Apis.Auth** - 1.68.0
- **Azure.AI.DocumentIntelligence** - 1.0.0
- **Azure.AI.OpenAI** - 2.1.0
- **MySql.EntityFrameworkCore** - 8.0.8
