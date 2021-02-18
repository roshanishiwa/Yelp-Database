import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('yelp_CptS451_2020/yelp_business.JSON','r') as f:  
        outfile =  open('.//business.txt', 'w')
        line = f.readline()
        count_line = 0
        outfile.write(str("HEADER: business_id, name, address, state, city, postal_code, latitude, longitude, stars, is_open\n"))
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id']) + '\t') #business id
            outfile.write(cleanStr4SQL(data['name']) + '\t') #name
            outfile.write(cleanStr4SQL(data['address']) + '\t') #full_address
            outfile.write(cleanStr4SQL(data['state']) + '\t') #state
            outfile.write(cleanStr4SQL(data['city']) + '\t') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + '\t')  #zipcode
            outfile.write(str(data['latitude']) + '\t') #latitude
            outfile.write(str(data['longitude']) + '\t') #longitude
            outfile.write(str(data['stars']) + '\t') #stars
            outfile.write(str(data['review_count']) + '\t') #reviewcount
            outfile.write(str(data['is_open']) + '\n') #openstatus

            # categories
            outfile.write('\t' + str("Categories: "))
            categories = data["categories"].split(', ')
            outfile.write(str(categories)+'\n')  #category list
            
            # attribute 
            outfile.write('\t'+str("Attributes: "))
            attributeData = parseAttribute(data['attributes'])
            outfile.write(str(attributeData))
            outfile.write('\n') 

            # hours 
            outfile.write('\t'+str("Hours: "))
            hoursData = parseHours(data['hours'])
            outfile.write(str(hoursData)) 
            
            outfile.write('\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

# parses attributes and their potential nested dictionaries
# returns a tuple of attributes
def parseAttribute(attributes):
    attributeData = []
    for key in attributes.keys():
        # if attribute value is dictionary we look through dictionary value
        if isinstance(attributes.get(key), dict):
            parseAttribute(attributes.get(key))
        # otherwise, we add to array
        else: 
            attributeData.append((cleanStr4SQL(key), cleanStr4SQL(attributes.get(key))))
    return attributeData

# parses hours data
# returns an array of the day and time in tuples
def parseHours(hours):
    hoursData = []
    for day, hour in hours.items():
        time1, time2 = hour.split("-")
        #outfile.write(str((day, [time1, time2]))+', ')
        hoursData.append((day, [time1, time2]))
    return hoursData

def parseUserData():
    #read the JSON file
    with open('yelp_CptS451_2020/yelp_user.JSON','r') as f:  
        outfile =  open('.//user.txt', 'w')
        line = f.readline()
        count_line = 0
        outfile.write(str("HEADER: user_id, name, yelping_since, tipcount, fans, averagestars, funny, useful, cool\n"))
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['user_id'])+'\t') #user_id
            outfile.write(cleanStr4SQL(data['name']) + '\t') #name
            outfile.write(cleanStr4SQL(data['yelping_since']) + '\t') #yelping since
            outfile.write(str(data['tipcount']) + '\t') #tipcount
            outfile.write(str(data['fans']) + '\t') #fans
            outfile.write(str(data['average_stars']) + '\t') #averagestars
            outfile.write(str(data['funny']) + '\t') #funny
            outfile.write(str(data['useful']) + '\t') #useful
            outfile.write(str(data['cool']) + '\n') #cool

            # friends
            outfile.write('\t' + str("Friends: "))
            friendsData = []
            for friend in data['friends']:
                friendsData.append(cleanStr4SQL(friend))
            outfile.write(str(friendsData))

            outfile.write('\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseCheckinData():
    #read the JSON file
    with open('yelp_CptS451_2020/yelp_checkin.JSON','r') as f:  
        outfile =  open('.//checkIn.txt', 'w')
        line = f.readline()
        count_line = 0
        outfile.write(str("HEADER: business_id: (year, month, day, time)\n"))
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+': \n') #business_id
            helpCheckin(data['date'], outfile)
            #outfile.write(str(checkinData))

            outfile.write('\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def helpCheckin(date, outfile):
    #checkin = []
    checkinData = date.split(',')
    for item in checkinData:
        time1, time2 = item.split(' ')
        year, month, day = time1.split('-')
        #checkin.append((year, month, day, time2))
        outfile.write(str((year, month, day, time2)) + "\t")
    #return checkin

def parseTipData():
    # read the JSON file
    with open('yelp_CptS451_2020/yelp_tip.JSON','r') as f:  
        outfile =  open('.//tip.txt', 'w')
        line = f.readline()
        count_line = 0
        outfile.write(str("HEADER: business_id, date, likes, text, user_id\n"))
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+': \n') #business_id
            outfile.write(cleanStr4SQL(data['date'])+'\t') #date
            outfile.write(str(data['likes'])+', \t') #likes
            outfile.write(cleanStr4SQL(data['text'])+'\t') #text
            outfile.write(cleanStr4SQL(data['user_id'])) #user_id

            outfile.write('\n')
            line = f.readline()
            count_line +=1       
    print(count_line)
    outfile.close()
    f.close()

if __name__ == "__main__":
    print('Business: ')
    parseBusinessData()
    print('User: ')
    parseUserData()
    print('Checkin: ')
    parseCheckinData()
    print('Tip: ')
    parseTipData()
