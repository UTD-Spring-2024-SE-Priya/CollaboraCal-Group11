import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation, json } from 'react-router-dom'; // Import useNavigate from react-router-dom
import './Main.css'; // Import Main.css for styling
import '../CalendarView/Calendar.css'; // Import Calendar.css for calendar styling
import user_icon from '../Assets/profilePicture.png';
import editIcon from '../Assets/edit.png';
import deleteIcon from '../Assets/trash.png'; // Add delete icon import
import * as req from '../../Requests';
import CalendarData from './CalendarSerialize';

const Main = () => {
  const navigate = useNavigate();
  const location = useLocation();
  //const [events, setEvents] = useState([]);

  const [calendarList, setCalendarList] = useState([])

  // useEffect(() => {
  //   // Check if location state contains event details
  //   if (location.state && location.state.title) {
  //     // Add the new event to the events array
  //     const { title, date, time, description } = location.state;
  //     setEvents([...events, { title, date, time, description }]);
  //   }

  //   console.log(events); // Log the events array

  // }, [location.state, events]); // Include events in the dependency array  

  const loadCalendars = async (response) => {
    if (response.status === 200) {
      let data = await response.json()
      let newArray = JSON.parse(data).map(x => new CalendarData(x.id, x.name, x.description))
      console.log(newArray)
      setCalendarList(newArray)
    }
    else {
      req.deleteAuthenticationCookie()
      navigate("/")
    }
  }

  useEffect(() => {
    let header = req.createAuthHeaders()
    req.getRequest("/getallcalendar", header)
      .then(loadCalendars)
      .catch(a => console.log(a))

  }, [])

  const handleCalendarButtonClick = () => {

    //setCalendarList([...calendarList, "ITEM"])

    navigate("/create-calendar");
  };

  const handleSignOut = () => {

    let header = req.createAuthHeaders()
    req.postRequest("/logout", header)
    req.deleteAuthenticationCookie()

    navigate("/"); // Redirect to the login/signup page
  };

  const DeleteCalendar = (calendarID) => {
    console.log("Deleting " + calendarID)

    let headers = req.createAuthHeaders()
    req.postRequest("/deletecalendar", headers, calendarID).catch(r => console.log(r));

    let newList = calendarList.filter(a => a.ID !== calendarID)
    setCalendarList(newList)
  }

  const LoadCalendar = (calendarID) => {
    navigate("/calendar", {state: {id: calendarID}})
  }


  const CalendarItems = () => {
    if (calendarList.length > 0) {
      return (
        <div>
          {calendarList.map(c => (

            <div className="calendarItem" >
              <div className="divisionDiv">
                <h1 className="AItemText">{c.Name}</h1>
                <p className="descriptionText">{c.Description}</p>
                <div className="darkStripeDiv">
                  <img src={deleteIcon} className="deleteButtonI" onClick={() => DeleteCalendar(c.ID)}></img>
                  <button className="viewButtonI" onClick={() => LoadCalendar(c.ID)}>View</button>
                </div>
              </div>
            </div>

          ))}
        </div>
      )
    }
    else {
      return (
        <h1>You have no calendars created</h1>
      )
    }
  }

  //End of calendar stuff
  const [EventCreation, CreateEvent] = useState("CreateEvent");


  return (
    //Div for the Header
    <div className="background">
      <div className="containerHeader">
        {/* First pair of columns */}
        <div className="headerColumnLeft">
          {/* Content for the first left column */}
        </div>
        <div className="headerColumnRight">
          {/* Content for the first right column */}
          <div className="signout-section">
            <button className="signoutButton" onClick={handleSignOut}>Sign Out</button>
            <div className="profile-picture">
              <img src={user_icon} alt="Profile" className="profile-img" />
            </div>
          </div>
        </div>
      </div>

      <div className="containerBody">
        <div className="bodyColumnLeft">
          {/* You can add any content you want for the left column */}
          <div className="textbody">Create</div>
          <div className="underline"></div>
          <div className='create-container'>
            <button className="eventButton" onClick={handleCalendarButtonClick}>Calendar</button>
          </div>

        </div>
        <div className="bodyColumnRight">
          <div className="invisibleFlexBox">
            <CalendarItems />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Main;
