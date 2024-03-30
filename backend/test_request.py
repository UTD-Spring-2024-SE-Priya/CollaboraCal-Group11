import requests

base_url = "http://localhost:5291/"

def postrequest(url, header):
    print(f"URL: {base_url + url}\nHeaders: {header}")
    return requests.post(base_url + url, headers=header)

def login(email, password):
    header = { 'EMail' : email, 'Password' : password }
    return postrequest("login", header)

def change_name(email, auth, newname):
    header = { 'EMail': email, 'Authentication' : auth, 'Name' : newname }
    return postrequest("changename", header)

def create_user(email, password, name):
    header = { 'EMail': email, 'Password': password, 'Name': name }
    return postrequest("newuser", header)

def testget():
    return requests.get(base_url)
