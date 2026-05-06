USE discogs;

-- Схема базы данных discogs:
--   artist        (ARTIST_ID, NAME, REALNAME, PROFILE, DATA_QUALITY)
--   `group`       (GROUP_ID, MAIN_ARTIST_ID, GROUP_ARTIST_ID)
--                  MAIN_ARTIST_ID  — группа/коллектив
--                  GROUP_ARTIST_ID — участник группы
--   namevariation (NAMEVARIATION_ID, ARTIST_ID, NAME)
--   `release`     (RELEASE_ID, MASTER_ID, STATUS, TITLE, COUNTRY,
--                  RELEASED, NOTES, DATA_QUALITY, IS_MAIN_RELEASE)
--   release_artist(RELEASE_ARTIST_ID, RELEASE_ID, ARTIST_ID,
--                  ANV, JOIN, ROLE, NAME, EXTRA_ARTIST)
--   style         (STYLE_ID, RELEASE_ID, MASTER_ID, STYLE_NAME)

------
--  Часть 1: Запросы и планы выполнения БЕЗ индексов
------

-- Запрос 1: найти исполнителя по имени
-- EXPLAIN: тип ALL (полное сканирование ~6 млн строк artist)

EXPLAIN
SELECT ARTIST_ID, NAME, REALNAME, PROFILE
FROM artist
WHERE NAME = 'Nirvana';

SELECT ARTIST_ID, NAME, REALNAME, PROFILE
FROM artist
WHERE NAME = 'Nirvana';

-- Запрос 2: участники музыкального коллектива по названию
-- EXPLAIN: тип ALL на `group` (~1.3 млн строк), затем eq_ref через PRIMARY на artist

EXPLAIN
SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

-- Запрос 3: все релизы исполнителя, отсортированные по дате
-- EXPLAIN: тип ALL на release_artist (~48 млн строк) — наиболее узкое место

EXPLAIN
SELECT a.NAME AS artist_name, r.TITLE, r.RELEASED
FROM artist         a
JOIN release_artist ra ON a.ARTIST_ID  = ra.ARTIST_ID
JOIN `release`      r  ON ra.RELEASE_ID = r.RELEASE_ID
WHERE a.NAME = 'Nirvana'
ORDER BY r.RELEASED;

-- Запрос 4: главные релизы указанного года с указанием стиля
-- EXPLAIN: тип ALL на style (~17 млн строк)

EXPLAIN
SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style      s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1
  AND YEAR(r.RELEASED)  = 1991;

SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style      s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1
  AND YEAR(r.RELEASED)  = 1991
LIMIT 10;

-- Запрос 5: исполнители по полнотекстовому поиску в профиле
-- До создания FULLTEXT-индекса используется LIKE (тип ALL, ~6 млн строк)

EXPLAIN
SELECT ARTIST_ID, NAME, PROFILE
FROM artist
WHERE PROFILE LIKE '%grunge%';

SELECT ARTIST_ID, NAME, PROFILE
FROM artist
WHERE PROFILE LIKE '%grunge%'
LIMIT 10;

------
--  Часть 2: Оценка времени выполнения запросов БЕЗ индексов
------

SET profiling = 1;

SELECT ARTIST_ID, NAME, REALNAME, PROFILE FROM artist WHERE NAME = 'Nirvana';

SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1 AND YEAR(r.RELEASED) = 1991
LIMIT 10;

SELECT ARTIST_ID, NAME, PROFILE FROM artist WHERE PROFILE LIKE '%grunge%' LIMIT 10;

SHOW PROFILES;
SET profiling = 0;

------
--  Часть 3: Создание индексов
------

-- Запрос 1, 2, 3: поиск исполнителя/группы по имени
CREATE INDEX idx_artist_name ON artist (NAME);

-- Запрос 2: поиск участников группы по MAIN_ARTIST_ID
CREATE INDEX idx_group_main_artist
    ON `group` (MAIN_ARTIST_ID);

-- Запрос 3: поиск релизов исполнителя (самая большая таблица — 48 млн строк)
CREATE INDEX idx_release_artist_artist
    ON release_artist (ARTIST_ID);

-- Запрос 4: фильтрация главных релизов по году
--   Функция YEAR() не позволяет использовать обычный индекс напрямую,
--   поэтому в запросе после создания индекса условие заменяется на диапазон.
CREATE INDEX idx_release_main_released
    ON `release` (IS_MAIN_RELEASE, RELEASED);

-- Запрос 4: поиск стиля по RELEASE_ID
CREATE INDEX idx_style_release
    ON style (RELEASE_ID);

-- Запрос 5: полнотекстовый поиск по профилю исполнителя
CREATE FULLTEXT INDEX ft_artist_profile
    ON artist (PROFILE);

------
--  Часть 4: Планы выполнения запросов ПОСЛЕ создания индексов
------

-- Запрос 1: поиск по имени — тип ref, используется idx_artist_name

EXPLAIN
SELECT ARTIST_ID, NAME, REALNAME, PROFILE
FROM artist
WHERE NAME = 'Nirvana';

SELECT ARTIST_ID, NAME, REALNAME, PROFILE
FROM artist
WHERE NAME = 'Nirvana';

-- Запрос 2: участники группы — idx_artist_name + idx_group_main_artist

EXPLAIN
SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

-- Запрос 3: релизы по исполнителю — idx_artist_name + idx_release_artist
-- Условие IS_MAIN_RELEASE заменено на диапазон дат для использования индекса.

EXPLAIN
SELECT a.NAME AS artist_name, r.TITLE, r.RELEASED
FROM artist         a
JOIN release_artist ra ON a.ARTIST_ID   = ra.ARTIST_ID
JOIN `release`      r  ON ra.RELEASE_ID = r.RELEASE_ID
WHERE a.NAME = 'Nirvana'
ORDER BY r.RELEASED;

SELECT a.NAME AS artist_name, r.TITLE, r.RELEASED
FROM artist         a
JOIN release_artist ra ON a.ARTIST_ID   = ra.ARTIST_ID
JOIN `release`      r  ON ra.RELEASE_ID = r.RELEASE_ID
WHERE a.NAME = 'Nirvana'
ORDER BY r.RELEASED;

-- Запрос 4: главные релизы года — диапазон вместо YEAR()
-- YEAR(RELEASED) = 1991 заменяется на RELEASED BETWEEN ... чтобы задействовать индекс.

EXPLAIN
SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style      s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1
  AND r.RELEASED BETWEEN '1991-01-01' AND '1991-12-31';

SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style      s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1
  AND r.RELEASED BETWEEN '1991-01-01' AND '1991-12-31'
LIMIT 10;

-- Запрос 5: полнотекстовый поиск — тип fulltext, ft_artist_profile

EXPLAIN
SELECT ARTIST_ID, NAME, PROFILE
FROM artist
WHERE MATCH(PROFILE) AGAINST ('grunge' IN BOOLEAN MODE);

SELECT ARTIST_ID, NAME, PROFILE
FROM artist
WHERE MATCH(PROFILE) AGAINST ('grunge' IN BOOLEAN MODE)
LIMIT 10;

------
--  Часть 5: Оценка времени выполнения запросов ПОСЛЕ индексов
------

SET profiling = 1;

SELECT ARTIST_ID, NAME, REALNAME, PROFILE FROM artist WHERE NAME = 'Nirvana';

SELECT a_member.NAME AS member_name
FROM `group`  g
JOIN artist a_band   ON g.MAIN_ARTIST_ID  = a_band.ARTIST_ID
JOIN artist a_member ON g.GROUP_ARTIST_ID = a_member.ARTIST_ID
WHERE a_band.NAME = 'Nirvana';

SELECT a.NAME AS artist_name, r.TITLE, r.RELEASED
FROM artist         a
JOIN release_artist ra ON a.ARTIST_ID   = ra.ARTIST_ID
JOIN `release`      r  ON ra.RELEASE_ID = r.RELEASE_ID
WHERE a.NAME = 'Nirvana'
ORDER BY r.RELEASED;

SELECT r.TITLE, r.RELEASED, s.STYLE_NAME
FROM `release` r
JOIN style s ON r.RELEASE_ID = s.RELEASE_ID
WHERE r.IS_MAIN_RELEASE = 1
  AND r.RELEASED BETWEEN '1991-01-01' AND '1991-12-31'
LIMIT 10;

SELECT ARTIST_ID, NAME, PROFILE
FROM artist
WHERE MATCH(PROFILE) AGAINST ('grunge' IN BOOLEAN MODE)
LIMIT 10;

SHOW PROFILES;
SET profiling = 0;


--- Drops

DROP INDEX idx_artist_name ON artist;

DROP INDEX idx_group_main_artist ON group;

DROP INDEX idx_release_artist_artist ON release_artist;

DROP INDEX idx_release_main_released ON release;

DROP INDEX idx_style_release ON style;

DROP INDEX ft_artist_profile ON artist;
