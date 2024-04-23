
const base_url = "http://localhost:5291"

function parseCookie(cname)
{
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for(let i = 0; i <ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
}

function setCookie(name, value, days)
{
    var cookie = name + "=" + value
    var expires = new Date()
    expires.setDate(expires.getDate() + days)

    if (days)
    {
        cookie += "; expires=" + expires.toUTCString()
    }

    document.cookie = cookie
}

function deleteCookie(cname)
{
    var date = new Date()
    date.setTime(date.getTime() - 1)
    document.cookie = cname + "=; expires=" + date.toUTCString()
}

export function deleteAuthenticationCookie()
{
    deleteCookie("email")
    deleteCookie("authentication")
}

export function setAuthenticationCookie(obj)
{
    deleteAuthenticationCookie()
    setCookie("email", obj.email, 7)
    setCookie("authentication", obj.authentication, 7)
}

export function doesCookieExist()
{
    if (parseCookie("email") === "") return false;
    if (parseCookie("authentication") === "") return false;
    return true
}

export function createAuthHeaders()
{
    let email = parseCookie("email")
    let auth = parseCookie("authentication")

    return {
        "Email" : email,
        "Authentication" : auth,
        "Content-type": "application/json; charset=UTF-8"
    }
}

export function createLoginHeaders(email, password)
{
    return {
        "Email" : email,
        "Password" : password,
        "Content-type": "application/json; charset=UTF-8"
    }
}

export function postRequest(endpoint, headers, body={})
{
    let url = base_url + endpoint
    console.log(url)
    return fetch(
        url,
        {
            method : "POST",
            headers : headers,
            body : JSON.stringify(body)
        }
    )
}

export function getRequest(endpoint, headers)
{
    let url = base_url + endpoint
    return fetch(
        url,
        {
            method : "GET",
            headers : headers,
        }
    )
}