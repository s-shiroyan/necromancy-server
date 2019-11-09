INSERT INTO `account` (id,name,normal_name,hash,mail,mail_verified,mail_verified_at,mail_token,password_token,state,last_login,created) VALUES 
(1,'admin','admin','$2a$10$q38QrceMiKLxI3rj6zGpHOQTqG61pEbu5wExo6bhV3s5srxbg11Oi','admin',0,NULL,NULL,NULL,1,NULL,'2019-11-09 20:23:33.2347542');
INSERT INTO `nec_soul` (id,account_id,name,level,created,password) VALUES 
(1,1,'Server',0,'2019-11-09 20:24:07.052087','0000');
INSERT INTO `nec_character` (id,account_id,soul_id,slot,map_id,x,y,z,name,race_id,sex_id,hair_id,hair_color_id,face_id,alignment_id,strength,vitality,dexterity,agility,intelligence,piety,luck,class_id,level,created) VALUES 
(1,1,1,1,1001902,23021.0,-224.0,0.0,'Admin',1,1,0,0,0,1,0,0,0,0,9,0,0,3,0,'2019-11-09 20:33:27.921302');