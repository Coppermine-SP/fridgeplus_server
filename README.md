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
    - [특정 아이템 삭제하기](#아이템-삭제하기)
    - [모든 아이템 삭제하기](#모든-아이템-삭제하기)
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
**Json Object Array**
|Key|Type|Value|
|-|-|-|
|name|string|요리 이름|
|description|string|요리 설명|
|steps|string[]|단계|

```json
{
    "name": "돼지고기 깻잎 볶음",
    "description": "냉장고에 있는 돼지고기 목심과 다양한 채소들을 활용한 깻잎 볶음을 만들어 보세요. 향긋한 깻잎과 함께 돼지고기를 볶아 맛있는 한 끼를 준비할 수 있습니다.",
    "steps": [
        "1. 준비된 돼지고기 목심을 손쉽게 먹기 좋은 크기로 얇게 썰어 주세요.",
        "2. 소량의 기름을 두른 팬을 중불로 예열합니다.",
        "3. 얇게 썬 돼지고기를 팬에 넣고 소금과 후추로 간을 하여 노릇해질 때까지 볶습니다.",
        "4. 돼지고기가 어느 정도 익으면 깐마늘과 송이버섯을 슬라이스 하여 팬에 추가합니다. 1-2분 정도 더 볶아 주세요.",
        "5. 청양고추를 잘게 썰어 넣어 매콤함을 더해줍니다.",
        "6. 마지막으로 깻잎을 손으로 찢어서 넣고 살짝 더 볶아줍니다. 깻잎은 오래 볶지 않도록 주의합니다.",
        "7. 모든 재료가 잘 섞이고 익으면 불을 끄고 접시에 담습니다.",
        "8. 기호에 따라 간장을 살짝 뿌려 맛을 더해줍니다."
    ]
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

#### 요청
**form-data**
|Key|Type|Value|
|-|-|-|
|id|int|아이템 ID|
```bash
curl -X POST 'https://fridgeplus.dev.cloudint.corp/api/fridge/deleteItem' \
-F 'id="172"'
```

#### 응답
**HTTP Status Code**

|Code|Description|
|-|-|
|200|삭제 성공|
|400|삭제 실패 (권한 없음)|

- - -

### 모든 아이템 삭제하기
사용자의 모든 아이템을 삭제합니다.
|Method|URL|인증|
|-|-|-|
|POST|api/fridge/reset|true|

#### 요청
**파라미터 없음**
```bash
curl -X GET 'https://fridgeplus.dev.cloudint.corp/api/fridge/reset'
```

#### 응답
**HTTP Status Code**

|Code|Description|
|-|-|
|200|삭제 성공|
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

```json
{"sub":"101433903511515263273"}
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
