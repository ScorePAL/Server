<div style="text-align: center;">
<h1>ScorePAL</h1>
<img alt="ScorePAL" src="assets/images/scorepal/logo-inline.png" >
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


#### MatchAllGet

```http
  GET /api/match/all
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `token` | `String` | - |
| `page` | `Long` | - |
| `limit` | `Long` | - |



#### MatchClubClubIdGet

```http
  GET /api/match/club/{clubId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `clubId` | `Long` | - |
| `token` | `String` | - |



#### MatchCreatePost

```http
  POST /api/match/create
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `String(string)` | - |



#### MatchMatchIdGet

```http
  GET /api/match/{matchId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `matchId` | `Long` | - |
| `token` | `String` | - |



#### MatchUpdateScoreMatchIdPut

```http
  PUT /api/match/update-score/{matchId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `matchId` | `Long` | - |
| `body` | `String(string)` | - |
| `scoreTeam1` | `Integer` | - |
| `scoreTeam2` | `Integer` | - |



#### 

```http
  GET /sse
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `clubId` | `Long` | - |



#### sseGet

```http
  GET /sse
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `clubId` | `Long` | - |



#### 

```http
  GET /api/team/all
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `token` | `String` | - |
| `page` | `Long` | - |
| `limit` | `Long` | - |



#### TeamAllGet

```http
  GET /api/team/all
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `token` | `String` | - |
| `page` | `Long` | - |
| `limit` | `Long` | - |



#### TeamCreatePost

```http
  POST /api/team/create
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `String(string)` | - |
| `name` | `String` | - |
| `clubId` | `Long` | - |



#### TeamDeleteIdDelete

```http
  DELETE /api/team/delete/{id}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `Long` | - |
| `token` | `String` | - |



#### TeamIdGet

```http
  GET /api/team/{id}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `Long` | - |
| `token` | `String` | - |



#### TeamUpdateIdPut

```http
  PUT /api/team/update/{id}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `Long` | - |
| `body` | `String(string)` | - |
| `name` | `String` | - |



#### 

```http
  POST /api/user/login
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `String(string)` | - |
| `email` | `String` | - |



#### UserLoginPost

```http
  POST /api/user/login
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `String(string)` | - |
| `email` | `String` | - |



#### UserNewTokenGet

```http
  GET /api/user/new-token
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `refreshtoken` | `String` | - |



#### UserRegisterPost

```http
  POST /api/user/register
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `body` | `String(string)` | - |
| `firstName` | `String` | - |
| `lastName` | `String` | - |
| `email` | `String` | - |
| `clubId` | `Long` | - |



#### UserTokenGet

```http
  GET /api/user/{token}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `token` | `String` | - |




## Feedback

If you have any feedback, please reach me using discord: @casahxd

## Contributors

<a href="https://github.com/scorepal/app/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=scorepal/server" />
</a>

Made with [contrib.rocks](https://contrib.rocks).