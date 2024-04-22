import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom'; // Import useNavigate from react-router-dom
import './Main.css'; // Import Main.css for styling
import '../CalendarView/Calendar.css'; // Import Calendar.css for calendar styling
import user_icon from '../Assets/profilePicture.png';
import editIcon from '../Assets/edit.png';
import deleteIcon from '../Assets/trash.png'; // Add delete icon import
import * as req from '../../Requests';

const Main = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [events, setEvents] = useState([]);
  //const [currentDate, setCurrentDate] = useState(new Date());
  const [calendars, setCalendars] = useState(['Calendar 1']); // Initial calendar
  const [editMode, setEditMode] = useState(Array(calendars.length).fill(false)); // Edit mode for each calendar
  const [checkboxColors, setCheckboxColors] = useState(Array(calendars.length).fill('#000000')); // Initial color for each checkbox
  const [showCreateEvent, setShowCreateEvent] = useState(false); // State to toggle CreateEvent visibility

  useEffect(() => {
    // Check if location state contains event details
    if (location.state && location.state.title) {
      // Add the new event to the events array
      const { title, date, time, description } = location.state;
      setEvents([...events, { title, date, time, description }]);
    }
    
    console.log(events); // Log the events array
    
  }, [location.state, events]); // Include events in the dependency array  

  const handleEventButtonClick = () => {
    navigate("/create-event");
  };

  const handleCalendarButtonClick = () => {
    navigate("/create-calendar");
  };

  const handleSignOut = () => {

    let header = req.createAuthHeaders()
    req.postRequest("/logout", header)
    req.deleteAuthenticationCookie()

    navigate("/"); // Redirect to the login/signup page
  };
  
  //Calendar 
  const [currentDate, setCurrentDate] = useState(new Date());

  const nextMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1));
  };

  const prevMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1));
  };

  const daysInMonth = (date) => {
    const year = date.getFullYear();
    const month = date.getMonth();
    return new Date(year, month + 1, 0).getDate();
  };

  const startDayOfMonth = (date) => {
    const year = date.getFullYear();
    const month = date.getMonth();
    return new Date(year, month, 1).getDay();
  };

  const addCalendar = () => {
    const newCalendarName = `Calendar ${calendars.length + 1}`;
    setCalendars([...calendars, newCalendarName]);
    setEditMode([...editMode, false]);
    setCheckboxColors([...checkboxColors, '#000000']);
  };

  const swagList = ["FIrst", "Second"]


  const CalendarItems = () => (
    <div>
      {swagList.map(c => (
        <div className="calendarItem">
          <h1>{c}</h1>
        </div>
      ))}  
    </div>
  )

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
            <button className="eventButton" onClick={handleEventButtonClick}>Event</button>
            <button className="calendarButton" onClick={addCalendar}>Calendar</button>
          </div>
          
        </div>
        <div className="bodyColumnRight">
          
          {/* <CalendarItems/> */}
          
        </div>  
      </div>
    </div>
  );
};

export default Main;
