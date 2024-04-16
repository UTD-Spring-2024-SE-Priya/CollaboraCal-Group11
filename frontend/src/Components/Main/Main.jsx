import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate from react-router-dom
import './Main.css'; // Import Main.css for styling
import './Calendar.css'; // Import Calendar.css for calendar styling
import user_icon from '../Assets/profilePicture.png';

const Main = () => {
  const navigate = useNavigate();

  const handleEventButtonClick = () => {
    navigate("/create-event");
  };

  const handleCalendarButtonClick = () => {
    navigate("/create-calendar");
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

  const renderCalendar = () => {
    const totalDays = daysInMonth(currentDate);
    const startDay = startDayOfMonth(currentDate);

    const days = [];

    let dayCount = 1;
    // Create table rows
    for (let i = 0; i < 5; i++) {
      const cells = [];
      // Create table cells for the days of the week
      for (let j = 0; j < 7; j++) {
        if (i === 0 && j < startDay) {
          // Empty cells for days before the start of the month
          cells.push(<td key={`empty-${j}`} className="empty-cell"></td>);
        } else if (dayCount <= totalDays) {
          // Cells with day numbers for the current month
          cells.push(
            <td key={`day-${dayCount}`} className="calendar-cell">
              <div className="calendar-number">{dayCount}</div>
            </td>
          );
          dayCount++;
        } else {
          // Empty cells for days after the end of the month
          cells.push(<td key={`empty-${j}`} className="empty-cell"></td>);
        }
      }
      // Create table row
      days.push(<tr key={`row-${i}`}>{cells}</tr>);
    }

    return days;
  };

  // Custom text for days of the week
  const daysOfWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
  
  // Custom text for months
  const months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];
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
            <button className="signoutButton">Sign Out</button>
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
          </div>
          <div classname='create-container'>
            <button className="calendarButton" onClick={handleCalendarButtonClick}>Calendar</button>
          </div>
          <div className="textbody2">My Calendars</div>
            <div className="underline"></div>
            <div className='textCalendars'>
              <div className="square1"></div> {/* New div for the square */}
              Calendar 1
            </div>
            <div className='textCalendars'>
              <div className="square2"></div> {/* New div for the square */}
              Calendar 2
            </div>
        </div>
        <div className="bodyColumnRight">
          {/* Calendar header */}
          <div className="calendar-header">
            <button onClick={prevMonth}>&lt;</button>
            <span className="calendar-title" colSpan="7">{months[currentDate.getMonth()]} {currentDate.getFullYear()}</span>
            <button onClick={nextMonth}>&gt;</button>
          </div>
          {/* Calendar table */}
          <table className="calendar">
            <thead>
              <tr>
                {daysOfWeek.map(day => (
                  <th key={day}>{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {renderCalendar()}
            </tbody>
          </table>
        </div>  
      </div>
    </div>
  );
};

export default Main;
