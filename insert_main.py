#CptS 451 - Spring 2021
# https://www.psycopg.org/docs/usage.html#query-parameters

import json
import psycopg2
import datetime
import time

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('yelp_CptS451_2020/yelp_business.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./yelp_business_out.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the current business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            sql_str = ("INSERT INTO Business (businessId, businessName, latitude, longitude, address, city, state, zip, numCheckins, numTips, isOpen, stars)"
                       + " VALUES ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}', '{7}', {8}, {9}, {10}, {11});").format(data['business_id'],cleanStr4SQL(data["name"]), data["latitude"], data["longitude"], cleanStr4SQL(data["address"]), data["city"], data["state"], data["postal_code"], 0 , 0 , [False,True][data["is_open"]], data["stars"])
            try:
                cur.execute(sql_str)              
            except Exception as e:
                print("Insert to Business failed!",e)
            conn.commit()            
            outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close() 
    f.close()

def insert2HoursTable():
    with open('yelp_CptS451_2020/yelp_business.JSON','r') as f:    
        outfile =  open('./yelp_hours_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            hours = parseHours(data['hours'], cleanStr4SQL(data['business_id']))

            #Generate the INSERT statement
            for val in hours:
                sql_str = val
                
                try:
                    cur.execute(sql_str)              
                except Exception as e:
                    print("Insert to Hours failed!",e)
                conn.commit()    
                outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

# parses hours data
# returns an array of the day and time in tuples
def parseHours(hours, business_id):
    hoursData = []
    for day, hour in hours.items():
        time1, time2 = hour.split("-")
        hours = ("INSERT INTO Hours ( businessID, dayOfWeek, open, close) " \
                "VALUES ('{0}', '{1}', '{2}', '{3}');").format(business_id , str(day), time1, time2)
        hoursData.append(hours)
    return hoursData

def insert2CategoryTable():
    with open('yelp_CptS451_2020/yelp_business.JSON','r') as f:    
        outfile =  open('./yelp_categories_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            #Generate the INSERT statement for the categories
            categories = data["categories"].split(', ')
            for val in categories:
                sql_str = "INSERT INTO Categories (businessID, categoryName) " \
                        "VALUES ('" +  cleanStr4SQL(data['business_id']) + "', '" + cleanStr4SQL(val) + "');"
                
                try:
                    cur.execute(sql_str)              
                except Exception as e:
                    print("Insert to Categories failed!",e)
                conn.commit()
                outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

def insert2AttributesTable():
    with open('yelp_CptS451_2020/yelp_business.JSON','r') as f:    
        outfile =  open('./yelp_attributes_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            temp = []
            attributes = parseAttribute(data['attributes'], data['business_id'], temp)

            #Generate the INSERT statement for the categories
            for val in attributes:
                sql_str = val
                
                try:
                    cur.execute(sql_str)              
                except Exception as e:
                    print("Insert to Attributes failed!",e)
                conn.commit()
                outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

# parses attributes and their potential nested dictionaries
# returns a tuple of attributes
def parseAttribute(attributes, business_id, attributeData):
    #attributeData = []
    #sql_str = ""
    for key in attributes.keys():
        # if attribute value is dictionary we look through dictionary value
        if isinstance(attributes.get(key), dict):
            parseAttribute(attributes.get(key), business_id, attributeData)
        # otherwise, we add to array
        else: 
            value = (attributes.get(key))
            #if value == "True":
                #boolValue = 1
            #else:
                #boolValue = 0
            sql_str = ("INSERT INTO Attributes (businessID, attributeName, value) " \
                        "VALUES ('{0}', '{1}', {2});").format(cleanStr4SQL(business_id), cleanStr4SQL(key), cleanStr4SQL(str(value)))
            attributeData.append(sql_str)
    return attributeData

def insert2UserTable():
    with open('yelp_CptS451_2020/yelp_user.JSON','r') as f:    
        outfile =  open('./yelp_user_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            timedate = yelpSince(data['yelping_since'])
            #Generate the INSERT statement for the USERS
            sql_str = ("INSERT INTO Users (userID, username, fans, average_stars, tipCount, funny, cool, useful, yelpSince, totalLikes, user_latitude, user_longitude)"
                       + " VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7}, '{8}', {9}, {10}, {11});").format(cleanStr4SQL(data['user_id']), cleanStr4SQL(data["name"]), data["fans"], data["average_stars"], data["tipcount"], data["funny"], data["cool"], data["useful"], datetime.datetime.fromtimestamp(timedate), 0, 0, 0)
                
            try:
                cur.execute(sql_str)              
            except Exception as e:
                print("Insert to Users failed!",e)
            conn.commit()
            outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

def yelpSince(date):
    date = str(date)
    time = datetime.datetime.strptime(date, '%Y-%m-%d %H:%M:%S')
    timestamp = datetime.datetime.timestamp(time)
    return timestamp

def insert2FriendsTable():
    with open('yelp_CptS451_2020/yelp_user.JSON','r') as f:    
        outfile =  open('./yelp_friends_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the Friend
            for friend in data['friends']:
                sql_str = ("INSERT INTO Friend (friendForID, friendOfID)" \
                       " VALUES ('" + cleanStr4SQL(data['user_id']) + "', '"  + str(friend) + "');")
                
                try:
                    cur.execute(sql_str)              
                except Exception as e:
                    print("Insert to Friend failed!",e)
                conn.commit()
                outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

def insert2TipTable():
    with open('yelp_CptS451_2020/yelp_tip.JSON','r') as f:    
        outfile =  open('./yelp_tip_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            timedate = helpTip(data['date'])
            #Generate the INSERT statement for the TIP
            sql_str = ("INSERT INTO Tip (userID, businessID, tipDate, tipText, likes)" \
                    " VALUES ('{0}', '{1}', '{2}', '{3}', {4});").format(cleanStr4SQL(data['user_id']),cleanStr4SQL(data['business_id']),datetime.datetime.fromtimestamp(timedate), cleanStr4SQL(data["text"]), data["likes"])

            try:
                cur.execute(sql_str)              
            except Exception as e:
                print("Insert to Tip failed!",e)
            conn.commit()
            outfile.write(sql_str + "\n")

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

def helpTip(date):
    date = str(date)
    time = datetime.datetime.strptime(date, "%Y-%m-%d %H:%M:%S")
    timestamp = datetime.datetime.timestamp(time)
    return timestamp

def insert2CheckinsTable():
    with open('yelp_CptS451_2020/yelp_checkin.JSON','r') as f:    
        outfile =  open('./yelp_checkin_out.SQL', 'w')
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Tigersrule5104'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            checkin = helpCheckin(data['date'], cleanStr4SQL(data['business_id']))
            #Generate the INSERT statement for the TIP
            for val in checkin:
                sql_str = val
                
                try:
                    cur.execute(sql_str)              
                except Exception as e:
                    print("Insert to Checkin failed!",e)
                conn.commit()
                outfile.write(sql_str + '\n')

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    outfile.close()  
    f.close()

def helpCheckin(date, business_id):
    checkin = []
    sql_str = ""
    checkinData = date.split(',')
    for item in checkinData:
        time1, time2 = item.split(' ')
        year, month, day = time1.split('-')
        #eT1 = datetime.datetime.strptime(time2, "%H:%M:%S")
        eT1 = datetime.time.fromisoformat(time2)
        time = eT1.isoformat(timespec='auto')
        #time = datetime.datetime.timestamp(eT1)
        sql_str = ("INSERT INTO Checkins (businessID, year, month, day, checkinTime) " \
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');").format(business_id, str(year), str(month), str(day), time)
        checkin.append(sql_str)
    return checkin



insert2UserTable()
insert2FriendsTable()
insert2BusinessTable()
insert2TipTable()
insert2CategoryTable()
insert2AttributesTable()
insert2HoursTable()
insert2CheckinsTable()