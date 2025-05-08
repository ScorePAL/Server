create schema if not exists ScorePAL;

create table clubs
(
    club_id  bigint auto_increment
        primary key,
    name     text null,
    logo_url text null
);

create table cups
(
    cup_id bigint auto_increment
        primary key,
    name   text null,
    scale  text null
);

create table players
(
    player_id  bigint auto_increment
        primary key,
    first_name text null,
    last_name  text null,
    birth_date date null
);

create table teams
(
    team_id bigint auto_increment
        primary key,
    name    text   null,
    club_id bigint null,
    constraint teams_ibfk_1
        foreign key (club_id) references clubs (club_id)
);

create table matches
(
    match_id     bigint auto_increment
        primary key,
    team1_id     bigint  null,
    team2_id     bigint  null,
    date         date    null,
    address      text    null,
    coach        text    null,
    match_state  text    null,
    started_time date    null,
    score_team1  tinyint null,
    score_team2  tinyint null,
    constraint matches_ibfk_1
        foreign key (team1_id) references teams (team_id),
    constraint matches_ibfk_2
        foreign key (team2_id) references teams (team_id)
);

create table cup_matches
(
    match_id bigint not null,
    cup_id   bigint not null,
    primary key (match_id, cup_id),
    constraint cup_matches_ibfk_1
        foreign key (match_id) references matches (match_id),
    constraint cup_matches_ibfk_2
        foreign key (cup_id) references cups (cup_id)
);

create index cup_id
    on cup_matches (cup_id);

create table match_histories
(
    match_history_id       bigint auto_increment
        primary key,
    match_event            text    null,
    play_time              tinyint null,
    additional_information text    null,
    match_id               bigint  null,
    constraint match_histories_ibfk_1
        foreign key (match_id) references matches (match_id)
);

create index match_id
    on match_histories (match_id);

create index team1_id
    on matches (team1_id);

create index team2_id
    on matches (team2_id);

create table played
(
    played_id        bigint auto_increment
        primary key,
    player_id        bigint     null,
    match_id         bigint     null,
    is_captain       tinyint(1) null,
    is_injured       tinyint(1) null,
    red_card         tinyint(1) null,
    yellow_card      tinyint(1) null,
    off_target_shots tinyint    null,
    on_target_shots  tinyint    null,
    blocked_shots    tinyint    null,
    goals            tinyint    null,
    entry_time       tinyint    null,
    exit_time        tinyint    null,
    jersey_number    tinyint    null,
    constraint played_ibfk_1
        foreign key (played_id) references players (player_id),
    constraint played_ibfk_2
        foreign key (match_id) references matches (match_id)
);

create table penalties
(
    penalty_id       bigint auto_increment
        primary key,
    played_id        bigint  null,
    result           text    null,
    obtaining_method text    null,
    penalty_time     tinyint null,
    constraint penalties_ibfk_1
        foreign key (played_id) references played (played_id)
);

create index played_id
    on penalties (played_id);

create index match_id
    on played (match_id);

create index club_id
    on teams (club_id);

create table users
(
    user_id    bigint auto_increment
        primary key,
    first_name text   null,
    last_name  text   null,
    role       text   null,
    created_at date   null,
    club       bigint null,
    constraint users_ibfk_1
        foreign key (club) references clubs (club_id)
);

create index club
    on users (club);

create table users_authentication
(
    user_id  bigint not null
        primary key,
    email    text   not null,
    salt     text   not null,
    password text   not null,
    constraint users_authentication_ibfk_1
        foreign key (user_id) references users (user_id)
);