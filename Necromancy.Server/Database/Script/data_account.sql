

INSERT INTO "account" ("id","name","normal_name","hash","mail","mail_verified","mail_verified_at","mail_token","password_token","state","last_login","created") 
VALUES 
(1,'admin','admin','$2a$10$q38QrceMiKLxI3rj6zGpHOQTqG61pEbu5wExo6bhV3s5srxbg11Oi','admin',0,NULL,NULL,NULL,100,NULL,'2019-11-09 20:23:33.2347542'),
 (2,'1234','1234','$2a$10$n.fk.CLJ1p1ZXULT/6wCFOCFtnUPQ77M97hZJWzRMgIkou/QFoQH6','1234',0,NULL,NULL,NULL,100,NULL,'2019-11-14 21:46:59.9196253'),
 (3,'ff','ff','$2a$10$TR9xGjx3T4UyBt/unPCr8OCgcK4H44h6ZsqHEX8UbOvxPTzgqmh6i','ff',0,NULL,NULL,NULL,100,NULL,'2019-11-15 09:09:18.4110375');
 INSERT INTO "nec_soul" ("id","account_id","name","level","created","password") 
VALUES 
(1,1,'Server',0,'2019-11-09 20:24:07.052087','0000'),
 (2,2,'Xeno.',20,'2019-11-14 21:47:47.886364','1234'),
 (3,3,'Unknown',20,'2019-11-15 09:10:50.0565622','0000');
 INSERT INTO "nec_character" ("id","account_id","soul_id","slot","map_id","x","y","z","name","race_id","sex_id","hair_id","hair_color_id","face_id","alignment_id","strength","vitality","dexterity","agility","intelligence","piety","luck","class_id","level","created") 
 VALUES 
 (1,1,1,1,1001902,23021.0,-224.0,0.0,'Admin',1,1,0,0,0,1,0,0,0,0,9,0,0,3,99,'2019-11-09 20:33:27.921302'),
 (2,2,2,1,1001007,2320.0,1850.0,25.0,'Talin',3,0,0,5,0,3,0,0,0,0,0,0,8,1,99,'2019-11-14 21:48:48.8749895'),
 (3,2,2,2,1001902,23021.0,-224.0,0.0,'Zakura',0,1,2,0,4,1,0,0,0,0,0,0,5,0,99,'2019-11-15 09:05:32.3891791'),
 (4,2,2,0,1001902,23021.0,-224.0,0.0,'Kadred',3,0,0,2,0,3,0,0,0,0,0,0,7,1,99,'2019-11-15 09:07:42.6628734'),
 (5,3,3,1,1001902,23021.0,-224.0,0.0,'One',4,1,1,5,0,1,0,0,0,0,0,0,8,3,99,'2019-11-15 09:12:49.2631598');