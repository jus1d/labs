PRAGMA foreign_keys = ON;

DROP TABLE IF EXISTS player_statistics;
DROP TABLE IF EXISTS player_characteristics;
DROP TABLE IF EXISTS players;

CREATE TABLE players (
    player_id TEXT PRIMARY KEY
);

CREATE TABLE player_characteristics (
    assessment_id INTEGER PRIMARY KEY,
    player_id TEXT NOT NULL,
    skill REAL,
    bankroll_mgmt REAL,
    tilt_control REAL,
    study_hours_week REAL,
    FOREIGN KEY (player_id) REFERENCES players(player_id)
);

CREATE TABLE player_statistics (
    result_id INTEGER PRIMARY KEY,
    assessment_id INTEGER NOT NULL UNIQUE,
    tournaments_played INTEGER,
    avg_field_size INTEGER,
    avg_buyin REAL,
    bankroll_buyins INTEGER,
    itm_rate REAL,
    final_table_rate REAL,
    win_rate REAL,
    biggest_cash_x REAL,
    final_bankroll_bi REAL,
    realized_roi REAL,
    went_broke INTEGER,
    profitable INTEGER,
    FOREIGN KEY (assessment_id) REFERENCES player_characteristics(assessment_id)
);

INSERT INTO players (player_id)
SELECT DISTINCT player_id
FROM poker_dataset;

INSERT INTO player_characteristics (
    assessment_id,
    player_id,
    skill,
    bankroll_mgmt,
    tilt_control,
    study_hours_week
)
SELECT
    rowid,
    player_id,
    skill,
    bankroll_mgmt,
    tilt_control,
    study_hours_week
FROM poker_dataset;

INSERT INTO player_statistics (
    result_id,
    assessment_id,
    tournaments_played,
    avg_field_size,
    avg_buyin,
    bankroll_buyins,
    itm_rate,
    final_table_rate,
    win_rate,
    biggest_cash_x,
    final_bankroll_bi,
    realized_roi,
    went_broke,
    profitable
)
SELECT
    rowid,
    rowid,
    tournaments_played,
    avg_field_size,
    avg_buyin,
    bankroll_buyins,
    itm_rate,
    final_table_rate,
    win_rate,
    biggest_cash_x,
    final_bankroll_bi,
    realized_roi,
    went_broke,
    profitable
FROM poker_dataset;

CREATE INDEX idx_player_characteristics_player_id
ON player_characteristics(player_id);

CREATE INDEX idx_player_statistics_assessment_id
ON player_statistics(assessment_id);
