-- 创建数据库
CREATE DATABASE IF NOT EXISTS MoodMusic;
USE MoodMusic;

-- 删除旧表（可选：防止重复）
DROP TABLE IF EXISTS songs;
DROP TABLE IF EXISTS moods;

-- 创建心情表
CREATE TABLE moods (
  id INT AUTO_INCREMENT PRIMARY KEY,
  mood_name VARCHAR(50) NOT NULL UNIQUE
);

-- 创建歌曲表
CREATE TABLE songs (
  id INT AUTO_INCREMENT PRIMARY KEY,
  title VARCHAR(100) NOT NULL,
  artist VARCHAR(100) NOT NULL,
  file_path VARCHAR(255),         -- 可用于本地mp3或YouTube链接
  youtube_url VARCHAR(255),       -- 新增字段：YouTube链接
  mood_id INT,
  FOREIGN KEY (mood_id) REFERENCES moods(id) ON DELETE CASCADE
);

-- 插入心情数据
INSERT INTO moods (mood_name) VALUES 
('happy'), 
('sad'), 
('chill'), 
('energy');

-- 插入 Happy 歌曲数据
INSERT INTO songs (title, artist, file_path, youtube_url, mood_id) VALUES
('Fifth Avenue', 'Walk off the Earth', 'assets/fifth-avenue.mp3', 'https://www.youtube.com/watch?v=ZThf5Q6ZiS4', (SELECT id FROM moods WHERE mood_name = 'happy')),
('Quarter Past Midnight', 'Bastille', 'assets/quarter-past-midnight.mp3', 'https://www.youtube.com/watch?v=F90Cw4l-8NY', (SELECT id FROM moods WHERE mood_name = 'happy')),
('No Shame', '5 Seconds of Summer', 'assets/no-shame.mp3', 'https://www.youtube.com/watch?v=IuYUBVhRXQ8', (SELECT id FROM moods WHERE mood_name = 'happy')),
('We Found Love', 'JAGMAC', 'assets/we-found-love.mp3', 'https://www.youtube.com/watch?v=tg00YEETFzg', (SELECT id FROM moods WHERE mood_name = 'happy')),
('400 Trillion', 'Justin Jesso', 'assets/400-trillion.mp3', 'https://www.youtube.com/watch?v=3T2o5slZh4I', (SELECT id FROM moods WHERE mood_name = 'happy')),
('This Time is Right', 'CVBZ, American Authors', 'assets/this-time-is-right.mp3', 'https://www.youtube.com/watch?v=VYOjWnS4cMY', (SELECT id FROM moods WHERE mood_name = 'happy')),
('Viva La Vida', 'Coldplay', 'assets/viva-la-vida.mp3', 'https://www.youtube.com/watch?v=dvgZkm1xWPE', (SELECT id FROM moods WHERE mood_name = 'happy')),
('Green Green Grass', 'George Ezra', 'assets/green-green-grass.mp3', 'https://www.youtube.com/watch?v=9xBXYvFTr4o', (SELECT id FROM moods WHERE mood_name = 'happy')),
('On My Way', 'Sheppard', 'assets/on-my-way.mp3', 'https://www.youtube.com/watch?v=2IF5TfnmV30', (SELECT id FROM moods WHERE mood_name = 'happy')),
('Happy Place (feat. Jasmine Thompson)', 'SAINT PHNX, Jasmine Thompson', 'assets/happy-place.mp3', 'https://www.youtube.com/watch?v=kcdCoiYh3Pg', (SELECT id FROM moods WHERE mood_name = 'happy')),
('Cheers', 'New Rules', 'assets/cheers.mp3', 'https://www.youtube.com/watch?v=tvTRZJ-4EyI', (SELECT id FROM moods WHERE mood_name = 'happy'));