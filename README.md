<div style="text-align: center;">
<h1>ScorePAL</h1>
<img alt="ScorePAL" src="https://raw.githubusercontent.com/ScorePAL/App/refs/heads/master/assets/images/scorepal/icon-inline.png" >
<p>A mobile app to manage a football team and analyse their statistics</p>
<a href="https://choosealicense.com/licenses/mit/"><img alt="MIT License" src="https://img.shields.io/badge/License-MIT-green.svg"></a>
<a href="https://opensource.org/licenses/"><img alt="GPLv3 License" src="https://img.shields.io/badge/License-GPL%20v3-yellow.svg"></a>
<a href="http://www.gnu.org/licenses/agpl-3.0"><img alt="AGPL License" src="https://img.shields.io/badge/License-MIT-blue.svg"></a>
</div>

## Tech Stack

ASP.Net (dotNet 8)

## Installation

Install ScorePAL by cloning both the server and the app repositories

```bash
# Cloning the app repository
git clone https://github.com/Vamos-Vamos/App.git

# Enter the app project
cd App

# Install the flutter dependencies
flutter pub get

# Cloning the server repository
git clone https://github.com/Vamos-Vamos/Server.git
```

## Deployement

```bash
# Enter the server project folder
cd Server/ScorePAL

# Build the docker image
docker build .

# Create a network to connect with the database
docker network create -d bridge web

# Enter the Server folder
cd ..

# Run the container
docker-compose -f compose.yml up
```

And voila your API is running in the background, you can manage it using Docker Desktop

## Roadmap

- User management
- Team statistics analysis
- User statistics
- Match History
- And much more

## API Routes
### Match-Update-score
```http
PUT /api/match/update-score/{matchId}
```
| Parameter | Type     | 
|-----------|----------|
| matchId | string |
| Body | Match |
### Match-{matchid}
```http
POST /api/match/{matchId}
```
| Parameter | Type     | 
|-----------|----------|
| matchId | string |
| Body | Match |
### Match-All
```http
GET /api/match/all
```
| Parameter | Type     | 
|-----------|----------|
| page | integer |
| limit | integer |
### Match-Create
```http
POST /api/match/create
```
| Parameter | Type     | 
|-----------|----------|
| Body | Match |
### Match-Club
```http
GET /api/match/club/{clubId}
```
| Parameter | Type     | 
|-----------|----------|
| clubId | string |
| Body | Club |
### SSE
```http
GET /sse
```
| Parameter | Type     | 
|-----------|----------|
| clubId | integer |
### Team-All
```http
GET /api/team/all
```
| Parameter | Type     | 
|-----------|----------|
| page | integer |
| limit | integer |
### Team-{id}
```http
POST /api/team/{id}
```
| Parameter | Type     | 
|-----------|----------|
| id | string |
| Body | Team |
### Team-Create
```http
POST /api/team/create
```
| Parameter | Type     | 
|-----------|----------|
| name | string |
| Body | Club |
### Team-Update
```http
PUT /api/team/update/{id}
```
| Parameter | Type     | 
|-----------|----------|
| id | string |
| Body | Team |
### User-Register
```http
POST /api/user/register
```
| Parameter | Type     | 
|-----------|----------|
| Body | UserRegister |
### User-Login
```http
POST /api/user/login
```
| Parameter | Type     | 
|-----------|----------|
| Body | UserLogin |
### User-Refresh-token
```http
GET /api/user/refresh-token
```
### User-Reset-password
```http
POST /api/user/reset-password
```
| Parameter | Type     | 
|-----------|----------|
| Body | string |