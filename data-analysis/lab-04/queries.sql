-- Enable foreign key enforcement.
PRAGMA foreign_keys = ON;

-- Show a sample of the source dataset.
SELECT *
FROM poker_dataset
LIMIT 10;

-- Count rows in all normalized source tables.
SELECT 'poker_dataset' AS table_name, COUNT(*) AS row_count
FROM poker_dataset
UNION ALL
SELECT 'players', COUNT(*)
FROM players
UNION ALL
SELECT 'player_characteristics', COUNT(*)
FROM player_characteristics
UNION ALL
SELECT 'player_statistics', COUNT(*)
FROM player_statistics;

-- Find characteristics without a matching player.
SELECT c.assessment_id, c.player_id
FROM player_characteristics AS c
LEFT JOIN players AS p ON p.player_id = c.player_id
WHERE p.player_id IS NULL;

-- Find statistics without matching characteristics.
SELECT s.result_id, s.assessment_id
FROM player_statistics AS s
LEFT JOIN player_characteristics AS c ON c.assessment_id = s.assessment_id
WHERE c.assessment_id IS NULL;

-- Calculate overall tournament performance metrics.
SELECT
    COUNT(*) AS observation_count,
    SUM(tournaments_played) AS total_tournaments,
    AVG(realized_roi) AS average_roi,
    MIN(avg_buyin) AS minimum_buyin,
    MAX(final_bankroll_bi) AS maximum_final_bankroll
FROM player_statistics;

-- Compare performance between profitable and unprofitable observations.
SELECT
    profitable,
    COUNT(*) AS player_observations,
    AVG(realized_roi) AS average_roi,
    AVG(final_bankroll_bi) AS average_final_bankroll
FROM player_statistics
GROUP BY profitable
ORDER BY average_roi DESC;

-- Calculate average ROI for each skill level.
SELECT
    CASE
        WHEN c.skill < 3 THEN 'low'
        WHEN c.skill < 6 THEN 'medium'
        WHEN c.skill < 8 THEN 'high'
        ELSE 'expert'
    END AS skill_level,
    COUNT(*) AS player_observations,
    AVG(s.realized_roi) AS average_roi
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
WHERE c.skill IS NOT NULL
GROUP BY skill_level
ORDER BY average_roi DESC;

-- Compare results across tournament-volume groups.
SELECT
    CASE
        WHEN s.tournaments_played < 50 THEN 'under_50'
        WHEN s.tournaments_played < 200 THEN '50_to_199'
        ELSE '200_or_more'
    END AS tournament_group,
    COUNT(*) AS player_observations,
    AVG(s.realized_roi) AS average_roi,
    AVG(s.itm_rate) AS average_itm_rate
FROM player_statistics AS s
GROUP BY tournament_group
ORDER BY player_observations DESC;

-- Show players with multiple observations.
SELECT
    p.player_id,
    COUNT(*) AS observation_count,
    AVG(s.realized_roi) AS average_roi
FROM players AS p
JOIN player_characteristics AS c ON c.player_id = p.player_id
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
GROUP BY p.player_id
HAVING COUNT(*) > 1
ORDER BY average_roi DESC;

-- Find observations with ROI above the overall average.
SELECT
    p.player_id,
    s.realized_roi,
    s.tournaments_played
FROM players AS p
JOIN player_characteristics AS c ON c.player_id = p.player_id
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
WHERE s.realized_roi > (
    SELECT AVG(realized_roi)
    FROM player_statistics
)
ORDER BY s.realized_roi DESC;

-- Find each player's highest-ROI observation.
SELECT
    c.player_id,
    s.assessment_id,
    s.realized_roi
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
WHERE s.realized_roi = (
    SELECT MAX(s2.realized_roi)
    FROM player_characteristics AS c2
    JOIN player_statistics AS s2 ON s2.assessment_id = c2.assessment_id
    WHERE c2.player_id = c.player_id
)
ORDER BY c.player_id, s.assessment_id;

-- Repeat the above-average ROI analysis using a CTE.
WITH average_roi AS (
    SELECT AVG(realized_roi) AS value
    FROM player_statistics
)
SELECT
    p.player_id,
    s.realized_roi,
    s.tournaments_played
FROM players AS p
JOIN player_characteristics AS c ON c.player_id = p.player_id
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
CROSS JOIN average_roi AS a
WHERE s.realized_roi > a.value
ORDER BY s.realized_roi DESC;

-- Select the ten players with the highest average ROI using sequential CTEs.
WITH player_roi AS (
    SELECT
        c.player_id,
        AVG(s.realized_roi) AS average_roi,
        SUM(s.tournaments_played) AS total_tournaments
    FROM player_characteristics AS c
    JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
    GROUP BY c.player_id
),
top_players AS (
    SELECT
        player_id,
        average_roi,
        total_tournaments
    FROM player_roi
    ORDER BY average_roi DESC
    LIMIT 10
)
SELECT *
FROM top_players
ORDER BY average_roi DESC;

-- Number each player's observations by assessment order.
SELECT
    c.player_id,
    c.assessment_id,
    s.tournaments_played,
    s.realized_roi,
    ROW_NUMBER() OVER (
        PARTITION BY c.player_id
        ORDER BY c.assessment_id
    ) AS observation_number
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
ORDER BY c.player_id, observation_number;

-- Rank players by average ROI within each skill level.
WITH skill_performance AS (
    SELECT
        CASE
            WHEN c.skill < 3 THEN 'low'
            WHEN c.skill < 6 THEN 'medium'
            WHEN c.skill < 8 THEN 'high'
            ELSE 'expert'
        END AS skill_level,
        c.player_id,
        AVG(s.realized_roi) AS average_roi
    FROM player_characteristics AS c
    JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
    WHERE c.skill IS NOT NULL
    GROUP BY skill_level, c.player_id
)
-- Return the ranking for each player.
SELECT
    skill_level,
    player_id,
    average_roi,
    DENSE_RANK() OVER (
        PARTITION BY skill_level
        ORDER BY average_roi DESC
    ) AS roi_rank
FROM skill_performance
ORDER BY skill_level, roi_rank, player_id;

-- Show each observation together with the player's average ROI.
SELECT
    c.player_id,
    c.assessment_id,
    s.realized_roi,
    AVG(s.realized_roi) OVER (
        PARTITION BY c.player_id
    ) AS player_average_roi
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
ORDER BY c.player_id, c.assessment_id;

-- Compare each ROI value with the previous observation of the same player.
SELECT
    c.player_id,
    c.assessment_id,
    s.realized_roi,
    LAG(s.realized_roi) OVER (
        PARTITION BY c.player_id
        ORDER BY c.assessment_id
    ) AS previous_observation_roi,
    s.realized_roi - LAG(s.realized_roi) OVER (
        PARTITION BY c.player_id
        ORDER BY c.assessment_id
    ) AS roi_change
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id
ORDER BY c.player_id, c.assessment_id;

-- Recreate the star-schema tables.
DROP TABLE IF EXISTS fact_player_performance;
DROP TABLE IF EXISTS dim_skill_level;
DROP TABLE IF EXISTS dim_player;

-- Create the player dimension.
CREATE TABLE dim_player (
    player_id TEXT PRIMARY KEY,
    average_skill REAL,
    average_bankroll_management REAL,
    average_tilt_control REAL,
    average_study_hours_week REAL,
    FOREIGN KEY (player_id) REFERENCES players(player_id)
);

-- Create the skill-level dimension.
CREATE TABLE dim_skill_level (
    skill_level_id INTEGER PRIMARY KEY,
    skill_level TEXT NOT NULL UNIQUE
);

-- Create the player-performance fact table.
CREATE TABLE fact_player_performance (
    assessment_id INTEGER PRIMARY KEY,
    player_id TEXT NOT NULL,
    skill_level_id INTEGER NOT NULL,
    tournaments_played INTEGER,
    avg_field_size INTEGER,
    avg_buyin REAL,
    itm_rate REAL,
    final_table_rate REAL,
    win_rate REAL,
    final_bankroll_bi REAL,
    realized_roi REAL,
    went_broke INTEGER,
    profitable INTEGER,
    FOREIGN KEY (assessment_id) REFERENCES player_characteristics(assessment_id),
    FOREIGN KEY (player_id) REFERENCES dim_player(player_id),
    FOREIGN KEY (skill_level_id) REFERENCES dim_skill_level(skill_level_id)
);

-- Load aggregated player attributes into the player dimension.
INSERT INTO dim_player (
    player_id,
    average_skill,
    average_bankroll_management,
    average_tilt_control,
    average_study_hours_week
)
SELECT
    p.player_id,
    AVG(c.skill),
    AVG(c.bankroll_mgmt),
    AVG(c.tilt_control),
    AVG(c.study_hours_week)
FROM players AS p
LEFT JOIN player_characteristics AS c ON c.player_id = p.player_id
GROUP BY p.player_id;

-- Load skill-level categories into the dimension.
INSERT INTO dim_skill_level (skill_level_id, skill_level)
VALUES
    (0, 'unknown'),
    (1, 'low'),
    (2, 'medium'),
    (3, 'high'),
    (4, 'expert');

-- Load performance measures into the fact table.
INSERT INTO fact_player_performance (
    assessment_id,
    player_id,
    skill_level_id,
    tournaments_played,
    avg_field_size,
    avg_buyin,
    itm_rate,
    final_table_rate,
    win_rate,
    final_bankroll_bi,
    realized_roi,
    went_broke,
    profitable
)
SELECT
    c.assessment_id,
    c.player_id,
    CASE
        WHEN c.skill IS NULL THEN 0
        WHEN c.skill < 3 THEN 1
        WHEN c.skill < 6 THEN 2
        WHEN c.skill < 8 THEN 3
        ELSE 4
    END,
    s.tournaments_played,
    s.avg_field_size,
    s.avg_buyin,
    s.itm_rate,
    s.final_table_rate,
    s.win_rate,
    s.final_bankroll_bi,
    s.realized_roi,
    s.went_broke,
    s.profitable
FROM player_characteristics AS c
JOIN player_statistics AS s ON s.assessment_id = c.assessment_id;

-- Report the five players with the highest average ROI.
SELECT
    f.player_id,
    AVG(f.realized_roi) AS average_roi,
    SUM(f.tournaments_played) AS total_tournaments,
    RANK() OVER (
        ORDER BY AVG(f.realized_roi) DESC
    ) AS roi_rank
FROM fact_player_performance AS f
GROUP BY f.player_id
ORDER BY roi_rank, f.player_id
LIMIT 5;

-- Report ROI, ITM rate, and profitability by skill level.
SELECT
    d.skill_level,
    COUNT(*) AS observation_count,
    ROUND(AVG(f.realized_roi), 2) AS average_roi,
    ROUND(AVG(f.itm_rate), 2) AS average_itm_rate,
    ROUND(100.0 * SUM(f.profitable) / COUNT(*), 2) AS profitable_share_percent
FROM fact_player_performance AS f
JOIN dim_skill_level AS d ON d.skill_level_id = f.skill_level_id
GROUP BY d.skill_level
ORDER BY average_roi DESC;

-- Report each skill level's share of estimated buy-in volume.
SELECT
    d.skill_level,
    ROUND(SUM(f.avg_buyin * f.tournaments_played), 2) AS estimated_buyin_volume,
    ROUND(
        100.0 * SUM(f.avg_buyin * f.tournaments_played)
        / SUM(SUM(f.avg_buyin * f.tournaments_played)) OVER (),
        2
    ) AS share_percent
FROM fact_player_performance AS f
JOIN dim_skill_level AS d ON d.skill_level_id = f.skill_level_id
GROUP BY d.skill_level
ORDER BY estimated_buyin_volume DESC;

-- Verify that no foreign key violations remain.
PRAGMA foreign_key_check;
