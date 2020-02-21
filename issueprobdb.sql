select s.*, ss.Name from stones s
join shapes ss on s.ShapeId = ss.id 
where s.companyid = 4

select s.*, ss.Name from stones s
join shapes ss on s.ShapeId = ss.id 
where s.companyid = 9

select * from stones where companyid = 9

select * from companies
select * from Vendors
select * from stones where Companyid <> 4
select * from findings where Companyid <> 4
select * from stones where companyid = 4
select * from vendors where companyid = 4
select * from shapes where companyId = 4
select * from findings where companyid = 4


select * from StyleStone

select * from stones
select * from findings
select * from shapes

select * from StyleCastings where CastingId = 9003
/*
begin transaction
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'17" CHAIN - 14W - W1614D-17-14W','17" Chain with SR',93.59,1.72,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'20" CHAIN - SS - W1921','20" Chain with LC',6.5,2.9,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'20" CHAIN - 14Y - W1921-14Y','20" Chain with LC',168,3.58,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'20" CHAIN - 14W - W1921-14W','20" Chain with LC',193.5,3.58,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'30" CHAIN - SS - W1617-30','30" Chain with SR',6.22,2.78,'NULL',9)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'30" CHAIN - 14Y - W1617-30-14Y','30" Chain with SR',180.26,3.48,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'30" CHAIN - 14W - W1617-30-14W','30" Chain with SR',187.43,3.48,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'17" CHAIN - 14W - 1025F-17','17" Chain with SR',43.57,0.8,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2005,'NUT - 14KW EN6','each',5.23,0.085,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2005,'POST - 14KW EPD3','each',3.3,0.04,'NULL',10)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2005,'POST - 14YEPD3','each',3.3,0.04,'NULL',24)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'EARWIRE .027 22/10 - 14Y - 646669','NULL',14.5,0.45,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'EARWIRE .027 22/10  - 14W - 626138','NULL',14.5,0.5,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14Y - .3mm - 20ga - 645825 .','NULL',2,0.02,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14W - .3mm - 20ga - 924272','NULL',3,0.03,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14Y - .4mm - 20ga - 645187','NULL',8,0.07,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14W - .4mm - 20ga - 649059 .','NULL',7,0.06,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - SS - .5 MM - 18ga - 693057','NULL',0.12,0.1,'NULL',12)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14Y - .5 MM - 18ga - 645187','NULL',10,0.07,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 14W - .5 MM - 18ga -N/A - use  649059','NULL',0,NULL,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'BALL POST JR - SS - 1943R-39477-S','NULL',1,0.08,'NULL',4)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'BALL POST JR - 14Y - 1943R-3006:S','NULL',5,0.044,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'BALL POST JR - 14W - 1943R-241057:S','NULL',6,0.048,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'CLICK IN EAR JR - SS - 29372:1007:P','NULL',22,0.0655,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'CLICK IN EAR JR - 14Y - 29372:1002:P','NULL',68,0.943,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'CLICK IN EAR JR - 14W - 29372:1001:P','NULL',60,0.852,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'EARWIRE BISH HK - 14Y - 23270:300196:S','NULL',15,0.39,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'EARWIRE BISH HK - 14W - 23270:300198:S','NULL',15,0.38,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'THREADER - 14Y - 23553:50004:S','NULL',19,0.41,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'THREADER - 14W - 23553:50005:S','NULL',20,0.41,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'SCROLL EAR TOP - SS - 24105:100003:S','NULL',10,0.67,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'SCROLL EAR TOP - 14Y - 24105:100001:S','NULL',29.5,0.85,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'SCROLL EAR TOP - 14W - 24105:100000:S','NULL',29.5,0.82,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - 4.1x2.9MM - SS - 695089','NULL',0.15,NULL,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'ROUND WIRE - 22ga - SS - 100352','NULL',0.33,NULL,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'JR - .5mm - 18ga - N/A - USE 649059','NULL',0,NULL,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'HARD WIRE - 22ga - 14W - WIRE:32182:P','NULL',3.85,NULL,'NULL',0)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2006,'20-22" CHAIN - SS 1420EX 20-21-22"','NULL',5,1.7,'NULL',32)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'19" CHAIN - SS - W1614D','NULL',0,NULL,'NULL',10)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'20" CHAIN - SS - W1614D','NULL',0,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'24" CHAIN - SS - W1614D','NULL',0,NULL,'NULL',4)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'15" CHAIN - SS - W1614D','NULL',0,NULL,'NULL',5)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'20" CHAIN - SS - W2508','NULL',0,NULL,'NULL',8)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'20" CHAIN - SS - 1624MCF','NULL',0,NULL,'NULL',4)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'17" CHAIN - SS - 1624MCF','NULL',0,NULL,'NULL',11)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'22" CHAIN - SS - 1624MCF','NULL',0,NULL,'NULL',5)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'17" CHAIN - SS - W1921','NULL',0,NULL,'NULL',6)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'18" CHAIN - SS - W1921','NULL',0,NULL,'NULL',5)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'20-22" CHAIN - SS - W1921EX','NULL',0,NULL,'NULL',26)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'64" CHAIN - SS - 1420','NULL',0,NULL,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'7" CHAIN - SS - W2908','NULL',6,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2004,'18" CHAIN - SS - 692504N8','NULL',0,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'18" CHAIN - SS - 1.5mm bead chain','NULL',0,NULL,'NULL',133)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'16" CHAIN - SS - Bead Chain','NULL',0,NULL,'NULL',1)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'LOBSTER CLAW - 8.25 x 3.25','NULL',0,NULL,'NULL',4)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'NUT - SILICONE','NULL',0,NULL,'NULL',102)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'NUT - 14KW - 5.20mm - TENSION BACK','NULL',0,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'NUT - PALL - EARRING SCREW BACK','NULL',0,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'POST - 14KW - STANDARD POST','NULL',0,NULL,'NULL',2)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'JR - SS - 4.5mm - 18ga','NULL',0,NULL,'NULL',29)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'JR - SS - 3.5mm - 18ga','NULL',0,NULL,'NULL',49)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2009,'JR - SS - 4.0mm - 16ga','NULL',0,NULL,'NULL',154)
INSERT INTO FINDINGS (CompanyID, VendorId, Name, [Desc], Price, Weight, Note, Qty) VALUES(4,2007,'STER; 7X5 SOLITAIRE SLIDE PDT, 22027:268201:S','NULL',25,NULL,'NULL',0)
commit tra nsaction



begin transaction
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','AM',NULL,'3.5','2002',3.8,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','AM',NULL,'6','2004',3.05,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','AM',NULL,'10','2004',14.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','AM',NULL,'6','2003',6.28,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','AM',NULL,'7x5','2005',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst-green','GA',NULL,'5','2002',2,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst-pink','PA',NULL,'5','2002',0.45,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'apatite','OT',NULL,'7x5','2005',50,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'2','2002',3,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'2.5','2002',5.34,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'3.5','2002',10,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'3.8','2002',17,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ+',NULL,'5','2002',42,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'6x4','3002',22.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','NULL',NULL,'7x5','3002',34,8,0,'Ande Gem','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ',NULL,'7x5','2005',63,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','NULL',NULL,'10x8','2005',36.64,12,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'aquamarine','AQ+',NULL,'3','2002',10,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'2','2002',0.2,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'4','2002',0.4,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'6','2002',2.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'4','2007',1,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'5','2007',0.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'black spinel','BS',NULL,'6','2007',1,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'blue chalcedony','OT',NULL,'6','2007',2.25,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'blue goldstone','OT',NULL,'10','2004',0.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'blue labradorite','OT',NULL,'7','2002',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'blue spinel','OT',NULL,'7x6','2005',2.05,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'zircon-blue','NULL',NULL,'7x5','2005',45,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'zircon-chocolate','NULL',NULL,'5','2002',12,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'zircon-chocolate','NULL',NULL,'6','2002',25,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','NULL',NULL,'2','2002',1,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'3.5','2002',3,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'6','2004',4,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'6','2007',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'5','2003',5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'7x5','2003',4.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'6x3','3002',2,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'7x5','3002',11,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'citrine','CI',NULL,'7x5','2005',5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'2','2002',6.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'3','2002',3,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'3','2002',20.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'5','2002',8,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'5','2003',130,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cr alexandrite','AL',NULL,'6','2003',188,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cz','CZ',NULL,'4','2002',0.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'cz','CZ',NULL,'5','2002',1.3,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'diamond','DI',NULL,'2.5','2002',32,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald','EM',NULL,'2','2002',11,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald','EM',NULL,'6','2004',52,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-created','EM-CR',NULL,'3','2002',22,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-created','EM-CR',NULL,'6','2002',168,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-created','EM-CR',NULL,'7','2002',250,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-created','EM-CR',NULL,'5','2003',110,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-imitation','EM-IM',NULL,'5','2002',6.25,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'emerald-imitation','EM-IM',NULL,'5','2003',12.1,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'2','2002',0.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'3','2002',0.6,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'4','2002',0.18,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'5','2004',1.7,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'6','2004',4.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'10','2004',3.79,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'6','2007',1.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'7','2007',2,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'6x4','2003',2.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'7','2003',6.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'7x5','2003',7.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet','GN',NULL,'6x4','3002',0.51,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-Maralambo','GN-MA',NULL,'7','2003',43.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-Orissa','GN-OR',NULL,'6x4','3002',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-Orissa','NULL',NULL,'7x5','3002',5.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-spessartite garnet','GN-SP',NULL,'3','2002',3.29,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-spessartite garnet','GN-SP',NULL,'7x5','3002',40,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-tsavorite','GN-TS',NULL,'3','2002',3.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-tsavorite','GN-TS',NULL,'5','2002',75.3,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'2','2002',0.8,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'3','2002',1.43,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'3.5','2002',0.95,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'4','2002',3.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'6','2004',7,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'5','2003',7.4,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'4x2','3002',4,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'iolite','LO',NULL,'7x5','3002',19,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'kunzite','KZ',NULL,'5','2002',27,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'kunzite','KZ',NULL,'6','2002',21,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'kunzite','KZ',NULL,'7','2002',36.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'kunzite','KZ',NULL,'10x8','2005',20,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'labradorite','LB',NULL,'7','2002',2,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'labradorite','LB',NULL,'10','2004',4.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'labradorite','LB',NULL,'7x5','3002',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'lapis','LA',NULL,'10','2004',5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'lavender spinel','OT',NULL,'6','2002',32,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'2.5','2002',1.75,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'3.5','2002',1.9,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'6','2004',12.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','NULL',NULL,'6','2003',3.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'6x4','2003',3.78,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'8x6','3002',16,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'10x8','2008',15,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'london blue topaz','LT',NULL,'20x3','3003',23,0,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-green','MO-G',NULL,'6','2004',4.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-white','MO-W',NULL,'6','2004',1.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-white','MO-W',NULL,'10','2004',5.7,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'morganite','MG',NULL,'4','2002',3.6,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'morganite','MG',NULL,'6','2002',9.16,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'morganite','MG',NULL,'4.5','2003',6.32,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'morganite','MG',NULL,'7x5','3002',13,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'mother of pearl','OT',NULL,'6','2004',0.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'mother of pearl','OT',NULL,'7','2007',0.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'mystic topaz','OT',NULL,'6x4','2005',0.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'opal','OP',NULL,'10x8','2005',40,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'opal-Ethiopian','OP-ET',NULL,'6','2004',6.25,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'2','2002',1,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'3.5','2002',1.75,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'6','2002',4,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'6','2007',10,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'5','2003',3,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'6','2003',7.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'6x4','2003',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'7','2003',20,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'7x5','2003',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'6x4','3002',3.25,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'8x6','3002',26,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'peridot','PE',NULL,'7x5','2005',15,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-rainbow','NULL',NULL,'4','2002',2.4,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-rainbow','NULL',NULL,'5','2004',4,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-rainbow','NULL',NULL,'7x5','3002',18,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'2','2002',0.8,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'2.5','2002',1,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'3','2002',1.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'3.5','2002',1.8,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'4','2002',2,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'6','2002',9,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'4','2003',6.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'6','2003',9.78,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'6x4','3002',1.6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'garnet-rhodolite','NULL',NULL,'8x6','3002',30,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'rose quartz','RQ',NULL,'6','2004',1,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'rose quartz','RQ',NULL,'10','2004',0.8,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'rose quartz','RQ',NULL,'6','2007',2,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-created','RU-CR',NULL,'3','2002',35,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-created','RU-CR',NULL,'5','2003',120,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-created','RU-CR',NULL,'6','2003',257,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-imitation','RU-IM',NULL,'5','2002',4.3,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-imitation','RU-IM',NULL,'7','2002',8.28,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'ruby-imitation','RU-IM',NULL,'5','2003',10.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire','SA',NULL,'2.5','2002',2.5,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire - A','NULL',NULL,'4','2002',56,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire','SA',NULL,'4','2003',45,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-created','SA-CR',NULL,'3','2002',30,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-created','SA-CR',NULL,'7','2002',335,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-imitation','SA-IM',NULL,'5','2003',8.6,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-Montana','SA-M',NULL,'3','2002',24.75,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-pink','PS',NULL,'3','2002',2,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-pink','PS',NULL,'5','2002',73,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-pink','PS',NULL,'4','2003',34,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-pink','PS',NULL,'5','2003',34,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-purple','SA-P',NULL,'5','2003',104,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-white','OT',NULL,'3','2002',10,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-yellow','OT',NULL,'3','2002',24.75,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-yellow','OT',NULL,'5','2003',125,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sky blue topaz','ST',NULL,'5','2002',5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sky blue topaz','ST',NULL,'6','2002',6,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sky blue topaz','ST',NULL,'10','2004',27.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sky blue topaz','ST',NULL,'10x8','2008',8,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'smokey quartz','SQ',NULL,'3','2002',0.7,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'smokey quartz','SQ',NULL,'5','2002',1.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'smokey quartz','SQ',NULL,'6','2002',1.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'2','2002',1.44,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'2.5','2002',1,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'3','2002',0.69,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'4','2002',5.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'5','2002',5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'6','2002',3,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'7','2002',2.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'4','2003',5.5,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'swiss blue topaz','IT',NULL,'7x5','2005',6.7,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'3','2002',6,3,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'5','2002',19,7.5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'5','2003',56,7.5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'8x6.5','2003',240,12,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'7','2003',110,12,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tanzanite','TA',NULL,'8x6','3002',22,12,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2002',22,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2002',33,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2002',25,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2003',25,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2003',45,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2003',20,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'5','2003',35,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'6','2002',100,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'7x5','2003',50,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-green','GT',NULL,'7x5','3002',30,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-indicolite','IT',NULL,'6x5','2003',135,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-peach','ET',NULL,'5','2003',25,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'2','2002',3.25,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'5','2002',35,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'6','2002',100,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'6','3005',52,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'7','2003',120,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink','PT',NULL,'7x5','2005',35,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink-imitation','PT-IM',NULL,'5','2003',10.8,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline-pink-imitation','PT-IM',NULL,'7','2002',8,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'turquoise','OT',NULL,'10','2004',20,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'white topaz','WT',NULL,'2','2002',0.84,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'white topaz','WT',NULL,'6','2002',4,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'white topaz','WT',NULL,'6','2003',4,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'white topaz','WT',NULL,'7x5','2005',1.75,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'white topaz','WT',NULL,'4','2002',0.55,5,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','NULL',NULL,'10x7','3006',0.73,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2007,'cr alexandrite - chatham','NULL',NULL,'7','2002',285,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'amethyst','NULL',NULL,'1.5','2002',4,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'sapphire-created','NULL',NULL,'6mm','2002',215,10,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'tourmaline','NULL',NULL,'8','2002',56,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moissanite','NULL',NULL,'1.5','2002',4,2,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'moonstone-rainbow','NULL',NULL,'6x4','2003',9.5,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'topaz - white','NULL',NULL,'10x8','2005',36,8,0,'NULL','NULL')
INSERT INTO STONES (CompanyId, VendorId, Name, [Desc], CtWt, StoneSize, ShapeId, Price, SettingCost, Qty, Note, Label) VALUES(4,2009,'topaz - ice blue','NULL',NULL,'11x9','2005',42.23,8,0,'NULL','NULL')
comm it transaction

begin transaction
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=3.8 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='6' AND ShapeId='2004' AND Price=3.05 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='10' AND ShapeId='2004' AND Price=14.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='6' AND ShapeId='2003' AND Price=6.28 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst-green' AND StoneSize ='5' AND ShapeId='2002' AND Price=2 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst-pink' AND StoneSize ='5' AND ShapeId='2002' AND Price=0.45 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='apatite' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=50 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='2' AND ShapeId='2002' AND Price=3 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=5.34 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=10 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='3.8' AND ShapeId='2002' AND Price=17 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='5' AND ShapeId='2002' AND Price=42 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='6x4' AND ShapeId='3002' AND Price=22.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=34 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=63 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='10x8' AND ShapeId='2005' AND Price=36.64 AND SettingCost =12 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='aquamarine' AND StoneSize ='3' AND ShapeId='2002' AND Price=10 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='2' AND ShapeId='2002' AND Price=0.2 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='4' AND ShapeId='2002' AND Price=0.4 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='6' AND ShapeId='2002' AND Price=2.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='4' AND ShapeId='2007' AND Price=1 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='5' AND ShapeId='2007' AND Price=0.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='black spinel' AND StoneSize ='6' AND ShapeId='2007' AND Price=1 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='blue chalcedony' AND StoneSize ='6' AND ShapeId='2007' AND Price=2.25 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='blue goldstone' AND StoneSize ='10' AND ShapeId='2004' AND Price=0.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='blue labradorite' AND StoneSize ='7' AND ShapeId='2002' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='blue spinel' AND StoneSize ='7x6' AND ShapeId='2005' AND Price=2.05 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='zircon-blue' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=45 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='zircon-chocolate' AND StoneSize ='5' AND ShapeId='2002' AND Price=12 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='zircon-chocolate' AND StoneSize ='6' AND ShapeId='2002' AND Price=25 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='2' AND ShapeId='2002' AND Price=1 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=3 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='6' AND ShapeId='2004' AND Price=4 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='6' AND ShapeId='2007' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='5' AND ShapeId='2003' AND Price=5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='7x5' AND ShapeId='2003' AND Price=4.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='6x3' AND ShapeId='3002' AND Price=2 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=11 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='citrine' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='2' AND ShapeId='2002' AND Price=6.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='3' AND ShapeId='2002' AND Price=3 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='3' AND ShapeId='2002' AND Price=20.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='5' AND ShapeId='2002' AND Price=8 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='5' AND ShapeId='2003' AND Price=130 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cr alexandrite' AND StoneSize ='6' AND ShapeId='2003' AND Price=188 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cz' AND StoneSize ='4' AND ShapeId='2002' AND Price=0.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='cz' AND StoneSize ='5' AND ShapeId='2002' AND Price=1.3 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='diamond' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=32 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald' AND StoneSize ='2' AND ShapeId='2002' AND Price=11 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald' AND StoneSize ='6' AND ShapeId='2004' AND Price=52 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-created' AND StoneSize ='3' AND ShapeId='2002' AND Price=22 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-created' AND StoneSize ='6' AND ShapeId='2002' AND Price=168 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-created' AND StoneSize ='7' AND ShapeId='2002' AND Price=250 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-created' AND StoneSize ='5' AND ShapeId='2003' AND Price=110 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-imitation' AND StoneSize ='5' AND ShapeId='2002' AND Price=6.25 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='emerald-imitation' AND StoneSize ='5' AND ShapeId='2003' AND Price=12.1 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='2' AND ShapeId='2002' AND Price=0.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='3' AND ShapeId='2002' AND Price=0.6 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='4' AND ShapeId='2002' AND Price=0.18 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='5' AND ShapeId='2004' AND Price=1.7 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='6' AND ShapeId='2004' AND Price=4.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='10' AND ShapeId='2004' AND Price=3.79 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='6' AND ShapeId='2007' AND Price=1.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='7' AND ShapeId='2007' AND Price=2 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='6x4' AND ShapeId='2003' AND Price=2.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='7' AND ShapeId='2003' AND Price=6.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='7x5' AND ShapeId='2003' AND Price=7.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet' AND StoneSize ='6x4' AND ShapeId='3002' AND Price=0.51 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-Maralambo' AND StoneSize ='7' AND ShapeId='2003' AND Price=43.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-Orissa' AND StoneSize ='6x4' AND ShapeId='3002' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-Orissa' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=5.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-spessartite garnet' AND StoneSize ='3' AND ShapeId='2002' AND Price=3.29 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-spessartite garnet' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=40 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-tsavorite' AND StoneSize ='3' AND ShapeId='2002' AND Price=3.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-tsavorite' AND StoneSize ='5' AND ShapeId='2002' AND Price=75.3 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='2' AND ShapeId='2002' AND Price=0.8 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='3' AND ShapeId='2002' AND Price=1.43 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=0.95 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='4' AND ShapeId='2002' AND Price=3.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='6' AND ShapeId='2004' AND Price=7 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='5' AND ShapeId='2003' AND Price=7.4 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='4x2' AND ShapeId='3002' AND Price=4 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='iolite' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=19 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='kunzite' AND StoneSize ='5' AND ShapeId='2002' AND Price=27 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='kunzite' AND StoneSize ='6' AND ShapeId='2002' AND Price=21 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='kunzite' AND StoneSize ='7' AND ShapeId='2002' AND Price=36.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='kunzite' AND StoneSize ='10x8' AND ShapeId='2005' AND Price=20 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='labradorite' AND StoneSize ='7' AND ShapeId='2002' AND Price=2 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='labradorite' AND StoneSize ='10' AND ShapeId='2004' AND Price=4.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='labradorite' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='lapis' AND StoneSize ='10' AND ShapeId='2004' AND Price=5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='lavender spinel' AND StoneSize ='6' AND ShapeId='2002' AND Price=32 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=1.75 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=1.9 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='6' AND ShapeId='2004' AND Price=12.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='6' AND ShapeId='2003' AND Price=3.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='6x4' AND ShapeId='2003' AND Price=3.78 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='8x6' AND ShapeId='3002' AND Price=16 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='10x8' AND ShapeId='2008' AND Price=15 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='london blue topaz' AND StoneSize ='20x3' AND ShapeId='3003' AND Price=23 AND SettingCost =0 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-green' AND StoneSize ='6' AND ShapeId='2004' AND Price=4.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-white' AND StoneSize ='6' AND ShapeId='2004' AND Price=1.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-white' AND StoneSize ='10' AND ShapeId='2004' AND Price=5.7 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='morganite' AND StoneSize ='4' AND ShapeId='2002' AND Price=3.6 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='morganite' AND StoneSize ='6' AND ShapeId='2002' AND Price=9.16 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='morganite' AND StoneSize ='4.5' AND ShapeId='2003' AND Price=6.32 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='morganite' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=13 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='mother of pearl' AND StoneSize ='6' AND ShapeId='2004' AND Price=0.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='mother of pearl' AND StoneSize ='7' AND ShapeId='2007' AND Price=0.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='mystic topaz' AND StoneSize ='6x4' AND ShapeId='2005' AND Price=0.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='opal' AND StoneSize ='10x8' AND ShapeId='2005' AND Price=40 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='opal-Ethiopian' AND StoneSize ='6' AND ShapeId='2004' AND Price=6.25 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='2' AND ShapeId='2002' AND Price=1 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=1.75 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='6' AND ShapeId='2002' AND Price=4 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='6' AND ShapeId='2007' AND Price=10 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='5' AND ShapeId='2003' AND Price=3 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='6' AND ShapeId='2003' AND Price=7.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='6x4' AND ShapeId='2003' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='7' AND ShapeId='2003' AND Price=20 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='7x5' AND ShapeId='2003' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='6x4' AND ShapeId='3002' AND Price=3.25 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='8x6' AND ShapeId='3002' AND Price=26 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='peridot' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=15 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-rainbow' AND StoneSize ='4' AND ShapeId='2002' AND Price=2.4 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-rainbow' AND StoneSize ='5' AND ShapeId='2004' AND Price=4 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-rainbow' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=18 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='2' AND ShapeId='2002' AND Price=0.8 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=1 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='3' AND ShapeId='2002' AND Price=1.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='3.5' AND ShapeId='2002' AND Price=1.8 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='4' AND ShapeId='2002' AND Price=2 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='6' AND ShapeId='2002' AND Price=9 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='4' AND ShapeId='2003' AND Price=6.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='6' AND ShapeId='2003' AND Price=9.78 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='6x4' AND ShapeId='3002' AND Price=1.6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='garnet-rhodolite' AND StoneSize ='8x6' AND ShapeId='3002' AND Price=30 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='rose quartz' AND StoneSize ='6' AND ShapeId='2004' AND Price=1 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='rose quartz' AND StoneSize ='10' AND ShapeId='2004' AND Price=0.8 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='rose quartz' AND StoneSize ='6' AND ShapeId='2007' AND Price=2 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-created' AND StoneSize ='3' AND ShapeId='2002' AND Price=35 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-created' AND StoneSize ='5' AND ShapeId='2003' AND Price=120 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-created' AND StoneSize ='6' AND ShapeId='2003' AND Price=257 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-imitation' AND StoneSize ='5' AND ShapeId='2002' AND Price=4.3 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-imitation' AND StoneSize ='7' AND ShapeId='2002' AND Price=8.28 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='ruby-imitation' AND StoneSize ='5' AND ShapeId='2003' AND Price=10.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=2.5 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire - A' AND StoneSize ='4' AND ShapeId='2002' AND Price=56 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire' AND StoneSize ='4' AND ShapeId='2003' AND Price=45 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-created' AND StoneSize ='3' AND ShapeId='2002' AND Price=30 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-created' AND StoneSize ='7' AND ShapeId='2002' AND Price=335 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-imitation' AND StoneSize ='5' AND ShapeId='2003' AND Price=8.6 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-Montana' AND StoneSize ='3' AND ShapeId='2002' AND Price=24.75 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-pink' AND StoneSize ='3' AND ShapeId='2002' AND Price=2 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-pink' AND StoneSize ='5' AND ShapeId='2002' AND Price=73 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-pink' AND StoneSize ='4' AND ShapeId='2003' AND Price=34 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-pink' AND StoneSize ='5' AND ShapeId='2003' AND Price=34 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-purple' AND StoneSize ='5' AND ShapeId='2003' AND Price=104 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-white' AND StoneSize ='3' AND ShapeId='2002' AND Price=10 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-yellow' AND StoneSize ='3' AND ShapeId='2002' AND Price=24.75 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-yellow' AND StoneSize ='5' AND ShapeId='2003' AND Price=125 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sky blue topaz' AND StoneSize ='5' AND ShapeId='2002' AND Price=5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sky blue topaz' AND StoneSize ='6' AND ShapeId='2002' AND Price=6 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sky blue topaz' AND StoneSize ='10' AND ShapeId='2004' AND Price=27.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sky blue topaz' AND StoneSize ='10x8' AND ShapeId='2008' AND Price=8 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='smokey quartz' AND StoneSize ='3' AND ShapeId='2002' AND Price=0.7 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='smokey quartz' AND StoneSize ='5' AND ShapeId='2002' AND Price=1.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='smokey quartz' AND StoneSize ='6' AND ShapeId='2002' AND Price=1.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='2' AND ShapeId='2002' AND Price=1.44 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='2.5' AND ShapeId='2002' AND Price=1 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='3' AND ShapeId='2002' AND Price=0.69 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='4' AND ShapeId='2002' AND Price=5.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='5' AND ShapeId='2002' AND Price=5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='6' AND ShapeId='2002' AND Price=3 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='7' AND ShapeId='2002' AND Price=2.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='4' AND ShapeId='2003' AND Price=5.5 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='swiss blue topaz' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=6.7 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='3' AND ShapeId='2002' AND Price=6 AND SettingCost =3 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='5' AND ShapeId='2002' AND Price=19 AND SettingCost =7.5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='5' AND ShapeId='2003' AND Price=56 AND SettingCost =7.5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='8x6.5' AND ShapeId='2003' AND Price=240 AND SettingCost =12 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='7' AND ShapeId='2003' AND Price=110 AND SettingCost =12 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tanzanite' AND StoneSize ='8x6' AND ShapeId='3002' AND Price=22 AND SettingCost =12 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2002' AND Price=22 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2002' AND Price=33 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2002' AND Price=25 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2003' AND Price=25 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2003' AND Price=45 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2003' AND Price=20 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='5' AND ShapeId='2003' AND Price=35 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='6' AND ShapeId='2002' AND Price=100 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='7x5' AND ShapeId='2003' AND Price=50 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-green' AND StoneSize ='7x5' AND ShapeId='3002' AND Price=30 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-indicolite' AND StoneSize ='6x5' AND ShapeId='2003' AND Price=135 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-peach' AND StoneSize ='5' AND ShapeId='2003' AND Price=25 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='2' AND ShapeId='2002' AND Price=3.25 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='5' AND ShapeId='2002' AND Price=35 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='6' AND ShapeId='2002' AND Price=100 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='6' AND ShapeId='3005' AND Price=52 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='7' AND ShapeId='2003' AND Price=120 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=35 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink-imitation' AND StoneSize ='5' AND ShapeId='2003' AND Price=10.8 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline-pink-imitation' AND StoneSize ='7' AND ShapeId='2002' AND Price=8 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='turquoise' AND StoneSize ='10' AND ShapeId='2004' AND Price=20 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='white topaz' AND StoneSize ='2' AND ShapeId='2002' AND Price=0.84 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='white topaz' AND StoneSize ='6' AND ShapeId='2002' AND Price=4 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='white topaz' AND StoneSize ='6' AND ShapeId='2003' AND Price=4 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='white topaz' AND StoneSize ='7x5' AND ShapeId='2005' AND Price=1.75 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='white topaz' AND StoneSize ='4' AND ShapeId='2002' AND Price=0.55 AND SettingCost =5 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='10x7' AND ShapeId='3006' AND Price=0.73 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2007 AND NAME='cr alexandrite - chatham' AND StoneSize ='7' AND ShapeId='2002' AND Price=285 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='amethyst' AND StoneSize ='1.5' AND ShapeId='2002' AND Price=4 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='sapphire-created' AND StoneSize ='6mm' AND ShapeId='2002' AND Price=215 AND SettingCost =10 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='tourmaline' AND StoneSize ='8' AND ShapeId='2002' AND Price=56 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moissanite' AND StoneSize ='1.5' AND ShapeId='2002' AND Price=4 AND SettingCost =2 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='moonstone-rainbow' AND StoneSize ='6x4' AND ShapeId='2003' AND Price=9.5 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='topaz - white' AND StoneSize ='10x8' AND ShapeId='2005' AND Price=36 AND SettingCost =8 AND Qty = 0
DELETE from Stones where CompanyId=4 AND VendorId = 2009 AND NAME='topaz - ice blue' AND StoneSize ='11x9' AND ShapeId='2005' AND Price=42.23 AND SettingCost =8 AND Qty = 0
commit transaction
*/
begin transaction
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '17" CHAIN - 14W - W1614D-17-14W' AND [Desc] = '17" Chain with SR' AND Price = 93.59 AND weight = 1.72 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - SS - W1921' AND [Desc] = '20" Chain with LC' AND Price = 6.5 AND weight = 2.9 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - 14Y - W1921-14Y' AND [Desc] = '20" Chain with LC' AND Price = 168 AND weight = 3.58 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - 14W - W1921-14W' AND [Desc] = '20" Chain with LC' AND Price = 193.5 AND weight = 3.58 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - SS - W1617-30' AND [Desc] = '30" Chain with SR' AND Price = 6.22 AND weight = 2.78 AND Qty = 9
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - 14Y - W1617-30-14Y' AND [Desc] = '30" Chain with SR' AND Price = 180.26 AND weight = 3.48 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - 14W - W1617-30-14W' AND [Desc] = '30" Chain with SR' AND Price = 187.43 AND weight = 3.48 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '17" CHAIN - 14W - 1025F-17' AND [Desc] = '17" Chain with SR' AND Price = 43.57 AND weight = 0.8 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'NUT - 14KW EN6' AND [Desc] = 'each' AND Price = 5.23 AND weight = 0.085 AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'POST - 14KW EPD3' AND [Desc] = 'each' AND Price = 3.3 AND weight = 0.04 AND Qty = 10
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'POST - 14YEPD3' AND [Desc] = 'each' AND Price = 3.3 AND weight = 0.04 AND Qty = 24
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'EARWIRE .027 22/10 - 14Y - 646669' AND [Desc] = 'NULL' AND Price = 14.5 AND weight = 0.45 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'EARWIRE .027 22/10  - 14W - 626138' AND [Desc] = 'NULL' AND Price = 14.5 AND weight = 0.5 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .3mm - 20ga - 645825 .' AND [Desc] = 'NULL' AND Price = 2 AND weight = 0.02 AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .3mm - 20ga - 924272' AND [Desc] = 'NULL' AND Price = 3 AND weight = 0.03 AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .4mm - 20ga - 645187' AND [Desc] = 'NULL' AND Price = 8 AND weight = 0.07 AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .4mm - 20ga - 649059 .' AND [Desc] = 'NULL' AND Price = 7 AND weight = 0.06 AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - SS - .5 MM - 18ga - 693057' AND [Desc] = 'NULL' AND Price = 0.12 AND weight = 0.1 AND Qty = 12
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .5 MM - 18ga - 645187' AND [Desc] = 'NULL' AND Price = 10 AND weight = 0.07 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .5 MM - 18ga -N/A - use  649059' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - SS - 1943R-39477-S' AND [Desc] = 'NULL' AND Price = 1 AND weight = 0.08 AND Qty = 4
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - 14Y - 1943R-3006:S' AND [Desc] = 'NULL' AND Price = 5 AND weight = 0.044 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - 14W - 1943R-241057:S' AND [Desc] = 'NULL' AND Price = 6 AND weight = 0.048 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - SS - 29372:1007:P' AND [Desc] = 'NULL' AND Price = 22 AND weight = 0.0655 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - 14Y - 29372:1002:P' AND [Desc] = 'NULL' AND Price = 68 AND weight = 0.943 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - 14W - 29372:1001:P' AND [Desc] = 'NULL' AND Price = 60 AND weight = 0.852 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'EARWIRE BISH HK - 14Y - 23270:300196:S' AND [Desc] = 'NULL' AND Price = 15 AND weight = 0.39 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'EARWIRE BISH HK - 14W - 23270:300198:S' AND [Desc] = 'NULL' AND Price = 15 AND weight = 0.38 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'THREADER - 14Y - 23553:50004:S' AND [Desc] = 'NULL' AND Price = 19 AND weight = 0.41 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'THREADER - 14W - 23553:50005:S' AND [Desc] = 'NULL' AND Price = 20 AND weight = 0.41 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - SS - 24105:100003:S' AND [Desc] = 'NULL' AND Price = 10 AND weight = 0.67 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - 14Y - 24105:100001:S' AND [Desc] = 'NULL' AND Price = 29.5 AND weight = 0.85 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - 14W - 24105:100000:S' AND [Desc] = 'NULL' AND Price = 29.5 AND weight = 0.82 AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 4.1x2.9MM - SS - 695089' AND [Desc] = 'NULL' AND Price = 0.15 AND weight = NULL AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'ROUND WIRE - 22ga - SS - 100352' AND [Desc] = 'NULL' AND Price = 0.33 AND weight = NULL AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - .5mm - 18ga - N/A - USE 649059' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'HARD WIRE - 22ga - 14W - WIRE:32182:P' AND [Desc] = 'NULL' AND Price = 3.85 AND weight = NULL AND Qty = 0
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20-22" CHAIN - SS 1420EX 20-21-22"' AND [Desc] = 'NULL' AND Price = 5 AND weight = 1.7 AND Qty = 32
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '19" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 10
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '24" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '15" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W2508' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 8
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 11
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '22" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 6
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20-22" CHAIN - SS - W1921EX' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 26
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '64" CHAIN - SS - 1420' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '7" CHAIN - SS - W2908' AND [Desc] = 'NULL' AND Price = 6 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = '18" CHAIN - SS - 692504N8' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - 1.5mm bead chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 133
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '16" CHAIN - SS - Bead Chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'LOBSTER CLAW - 8.25 x 3.25' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - SILICONE' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 102
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - 14KW - 5.20mm - TENSION BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - PALL - EARRING SCREW BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'POST - 14KW - STANDARD POST' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 29
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 3.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 49
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.0mm - 16ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 154
DELETE FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'STER; 7X5 SOLITAIRE SLIDE PDT, 22027:268201:S' AND [Desc] = 'NULL' AND Price = 25 AND weight = NULL AND Qty = 0
rollback transaction

select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME like 'JR - 14W - %' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 4.1x2.9MM - SS - 695089' AND [Desc] = 'NULL' AND Price = 0.15 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'ROUND WIRE - 22ga - SS - 100352' AND [Desc] = 'NULL' AND Price = 0.33 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - .5mm - 18ga - N/A - USE 649059' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'HARD WIRE - 22ga - 14W - WIRE:32182:P' AND [Desc] = 'NULL' AND Price = 3.85 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '19" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 10
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '24" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '15" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W2508' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 8
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 11
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '22" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 6
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20-22" CHAIN - SS - W1921EX' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 26
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '64" CHAIN - SS - 1420' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '7" CHAIN - SS - W2908' AND [Desc] = 'NULL' AND Price = 6 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = '18" CHAIN - SS - 692504N8' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - 1.5mm bead chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 133
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '16" CHAIN - SS - Bead Chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'LOBSTER CLAW - 8.25 x 3.25' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - SILICONE' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 102
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - 14KW - 5.20mm - TENSION BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - PALL - EARRING SCREW BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'POST - 14KW - STANDARD POST' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 29
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 3.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 49
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.0mm - 16ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 154
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'STER; 7X5 SOLITAIRE SLIDE PDT, 22027:268201:S' AND [Desc] = 'NULL' AND Price = 25 AND weight = NULL AND Qty = 0




select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '17" CHAIN - 14W - W1614D-17-14W' AND [Desc] = '17" Chain with SR' AND Price = 93.59 AND weight = 1.72 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - SS - W1921' AND [Desc] = '20" Chain with LC' AND Price = 6.5 AND weight = 2.9 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - 14Y - W1921-14Y' AND [Desc] = '20" Chain with LC' AND Price = 168 AND weight = 3.58 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20" CHAIN - 14W - W1921-14W' AND [Desc] = '20" Chain with LC' AND Price = 193.5 AND weight = 3.58 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - SS - W1617-30' AND [Desc] = '30" Chain with SR' AND Price = 6.22 AND weight = 2.78 AND Qty = 9
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - 14Y - W1617-30-14Y' AND [Desc] = '30" Chain with SR' AND Price = 180.26 AND weight = 3.48 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '30" CHAIN - 14W - W1617-30-14W' AND [Desc] = '30" Chain with SR' AND Price = 187.43 AND weight = 3.48 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '17" CHAIN - 14W - 1025F-17' AND [Desc] = '17" Chain with SR' AND Price = 43.57 AND weight = 0.8 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'NUT - 14KW EN6' AND [Desc] = 'each' AND Price = 5.23 AND weight = 0.085 AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'POST - 14KW EPD3' AND [Desc] = 'each' AND Price = 3.3 AND weight = 0.04 AND Qty = 10
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2005 AND NAME = 'POST - 14YEPD3' AND [Desc] = 'each' AND Price = 3.3 AND weight = 0.04 AND Qty = 24
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'EARWIRE .027 22/10 - 14Y - 646669' AND [Desc] = 'NULL' AND Price = 14.5 AND weight = 0.45 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'EARWIRE .027 22/10  - 14W - 626138' AND [Desc] = 'NULL' AND Price = 14.5 AND weight = 0.5 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .3mm - 20ga - 645825 .' AND [Desc] = 'NULL' AND Price = 2 AND weight = 0.02 AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .3mm - 20ga - 924272' AND [Desc] = 'NULL' AND Price = 3 AND weight = 0.03 AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .4mm - 20ga - 645187' AND [Desc] = 'NULL' AND Price = 8 AND weight = 0.07 AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .4mm - 20ga - 649059 .' AND [Desc] = 'NULL' AND Price = 7 AND weight = 0.06 AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - SS - .5 MM - 18ga - 693057' AND [Desc] = 'NULL' AND Price = 0.12 AND weight = 0.1 AND Qty = 12
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14Y - .5 MM - 18ga - 645187' AND [Desc] = 'NULL' AND Price = 10 AND weight = 0.07 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 14W - .5 MM - 18ga -N/A - use  649059' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - SS - 1943R-39477-S' AND [Desc] = 'NULL' AND Price = 1 AND weight = 0.08 AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - 14Y - 1943R-3006:S' AND [Desc] = 'NULL' AND Price = 5 AND weight = 0.044 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'BALL POST JR - 14W - 1943R-241057:S' AND [Desc] = 'NULL' AND Price = 6 AND weight = 0.048 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - SS - 29372:1007:P' AND [Desc] = 'NULL' AND Price = 22 AND weight = 0.0655 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - 14Y - 29372:1002:P' AND [Desc] = 'NULL' AND Price = 68 AND weight = 0.943 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'CLICK IN EAR JR - 14W - 29372:1001:P' AND [Desc] = 'NULL' AND Price = 60 AND weight = 0.852 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'EARWIRE BISH HK - 14Y - 23270:300196:S' AND [Desc] = 'NULL' AND Price = 15 AND weight = 0.39 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'EARWIRE BISH HK - 14W - 23270:300198:S' AND [Desc] = 'NULL' AND Price = 15 AND weight = 0.38 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'THREADER - 14Y - 23553:50004:S' AND [Desc] = 'NULL' AND Price = 19 AND weight = 0.41 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'THREADER - 14W - 23553:50005:S' AND [Desc] = 'NULL' AND Price = 20 AND weight = 0.41 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - SS - 24105:100003:S' AND [Desc] = 'NULL' AND Price = 10 AND weight = 0.67 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - 14Y - 24105:100001:S' AND [Desc] = 'NULL' AND Price = 29.5 AND weight = 0.85 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'SCROLL EAR TOP - 14W - 24105:100000:S' AND [Desc] = 'NULL' AND Price = 29.5 AND weight = 0.82 AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - 4.1x2.9MM - SS - 695089' AND [Desc] = 'NULL' AND Price = 0.15 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'ROUND WIRE - 22ga - SS - 100352' AND [Desc] = 'NULL' AND Price = 0.33 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = 'JR - .5mm - 18ga - N/A - USE 649059' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'HARD WIRE - 22ga - 14W - WIRE:32182:P' AND [Desc] = 'NULL' AND Price = 3.85 AND weight = NULL AND Qty = 0
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2006 AND NAME = '20-22" CHAIN - SS 1420EX 20-21-22"' AND [Desc] = 'NULL' AND Price = 5 AND weight = 1.7 AND Qty = 32
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '19" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 10
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '24" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '15" CHAIN - SS - W1614D' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - W2508' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 8
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 11
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '22" CHAIN - SS - 1624MCF' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '17" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 6
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - W1921' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 5
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '20-22" CHAIN - SS - W1921EX' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 26
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '64" CHAIN - SS - 1420' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '7" CHAIN - SS - W2908' AND [Desc] = 'NULL' AND Price = 6 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2004 AND NAME = '18" CHAIN - SS - 692504N8' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '18" CHAIN - SS - 1.5mm bead chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 133
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = '16" CHAIN - SS - Bead Chain' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 1
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'LOBSTER CLAW - 8.25 x 3.25' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 4
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - SILICONE' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 102
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - 14KW - 5.20mm - TENSION BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'NUT - PALL - EARRING SCREW BACK' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'POST - 14KW - STANDARD POST' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 2
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 29
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 3.5mm - 18ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 49
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2009 AND NAME = 'JR - SS - 4.0mm - 16ga' AND [Desc] = 'NULL' AND Price = 0 AND weight = NULL AND Qty = 154
select id, name FROM Findings where CompanyId = 4 AND VendorId = 2007 AND NAME = 'STER; 7X5 SOLITAIRE SLIDE PDT, 22027:268201:S' AND [Desc] = 'NULL' AND Price = 25 AND weight = NULL AND Qty = 0
