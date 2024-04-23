import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom'; // Import useNavigate from react-router-dom
import './Calendar.css'; // Import Calendar.css for calendar styling
import './CalendarView.css'; // Import CalendarView.css for calendar styling
import user_icon from '../Assets/profilePicture.png';
import editIcon from '../Assets/edit.png';
import deleteIcon from '../Assets/trash.png'; // Add delete icon import
import * as req from '../../Requests';
import EventData from './EventSerialize';

const CalendarView = ({ route }) => {
    const navigate = useNavigate();
    const location = useLocation();
    const [events, setEvents] = useState([]);

    //const [currentDate, setCurrentDate] = useState(new Date());
    //const [calendars, setCalendars] = useState(['Calendar 1']); // Initial calendar
    //   const [editMode, setEditMode] = useState(Array(calendars.length).fill(false)); // Edit mode for each calendar
    //   const [checkboxColors, setCheckboxColors] = useState(Array(calendars.length).fill('#000000')); // Initial color for each checkbox
    const [showCreateEvent, setShowCreateEvent] = useState(false); // State to toggle CreateEvent visibility

    // useEffect(() => {
    //     // Check if location state contains event details
    //     if (location.state && location.state.title) {
    //         // Add the new event to the events array
    //         const { title, date, time, description } = location.state;
    //         setEvents([...events, { title, date, time, description }]);

    //         console.log(events)
    //     }

    //     console.log(events); // Log the events array

    // }, [location.state, events]); // Include events in the dependency array  

    const loadEvents = async (response) => {
        if (response.status == 200) {
            let data = await response.json()

            // constructor(id, name, description, date, time)
            let newArray = JSON.parse(data).map(x => new EventData(x.id, x.name, x.description, x.start))
            setEvents(newArray)
        }
        else {
            console.log("Events failed to load")
            navigate("/home")
        }
    }

    const queryEvents = () => {


        /*
        [JsonProperty("start")] DateTime StartTime,
        [JsonProperty("end")] DateTime EndTime,
        [JsonProperty("calendarID")] int CalendarID
        */

        let headers = req.createAuthHeaders()
        let body = JSON.stringify({
            start: "1970-01-01 12:00:00",
            end: "2038-01-01 12:00:00",
            calendarID: location.state.id,
        })

        // console.log(body)

        req.postRequest("/getevents", headers, body)
            .then(loadEvents)
            .catch(r => console.log(r));

    }

    useEffect(() => {
        if (location.state == null) {
            navigate("/home")
            return;
        }
        if (location.state.id == null) {
            navigate("/home")
            return;
        }

        queryEvents()

    }, [])

    const handleEventButtonClick = () => {
        navigate("/create-event", { state: { id: location.state.id } });
    };

    const handleShareButtonClick = () => {
        navigate("/share", { state: { id: location.state.id } })
    }

    const handleCalendarButtonClick = () => {
        navigate("/create-calendar");
    };

    const handleSignOut = () => {

        let header = req.createAuthHeaders()
        req.postRequest("/logout", header)
        req.deleteAuthenticationCookie()

        navigate("/"); // Redirect to the login/signup page
    };

    const handleBackButton = () => {
        navigate("/home")
    }

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

    //   const addCalendar = () => {
    //     const newCalendarName = `Calendar ${calendars.length + 1}`;
    //     setCalendars([...calendars, newCalendarName]);
    //     setEditMode([...editMode, false]);
    //     setCheckboxColors([...checkboxColors, '#000000']);
    //   };

    //   const toggleEditMode = (index) => {
    //     const updatedEditMode = [...editMode];
    //     updatedEditMode[index] = !updatedEditMode[index];
    //     setEditMode(updatedEditMode);
    //   };

    //   const deleteCalendar = (index) => {
    //     const isConfirmed = window.confirm("Are you sure you want to delete this calendar?");
    //     if (isConfirmed) {
    //       const updatedCalendars = [...calendars];
    //       updatedCalendars.splice(index, 1);
    //       setCalendars(updatedCalendars);
    //       const updatedEditMode = [...editMode];
    //       updatedEditMode.splice(index, 1);
    //       setEditMode(updatedEditMode);
    //       const updatedColors = [...checkboxColors];
    //       updatedColors.splice(index, 1);
    //       setCheckboxColors(updatedColors);
    //     }
    //   };

    //   const handleColorChange = (index, color) => {
    //     const updatedColors = [...checkboxColors];
    //     updatedColors[index] = color;
    //     setCheckboxColors(updatedColors);
    //   };

    const deleteEvent = (id) => {

        let newArray = events.filter(a => a.ID !== id);
        setEvents(newArray);

        let headers = req.createAuthHeaders()
        req.postRequest("/deleteevent", headers, id)
            .catch(r => console.log(r))
    }

    // Update rendering logic to include events
    const renderCalendar = () => {
        const totalDays = daysInMonth(currentDate);
        const startDay = startDayOfMonth(currentDate);

        const days = [];

        let dayCount = 1;

        let iterDate = new Date(currentDate);

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

                    iterDate.setDate(dayCount)

                    const event = events.find(e => {
                        const eventDate = e.Date;

                        // console.log(`${eventDate.getMonth()}/${eventDate.getDate()}/${eventDate.getFullYear()}`)
                        // console.log(`${iterDate.getMonth()}/${iterDate.getDate()}/${iterDate.getFullYear()}`)

                        return eventDate.getDate() === iterDate.getDate()
                            && eventDate.getMonth() === iterDate.getMonth()
                            && eventDate.getFullYear() === iterDate.getFullYear();
                    });

                    cells.push(
                        <td key={`day-${dayCount}`} className="calendar-cell">
                            <div className="calendar-number">{dayCount}</div>
                            {event && (
                                <div className="event-container" onClick={() => deleteEvent(event.ID)}>
                                    <div className="event-title">{event.Name}</div>
                                    <div className="event-time">{event.Time}</div>
                                </div>
                            )}
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
                <div className="headerColumnLeftC">
                    {/* Content for the first left column */
                        <button className="backButton" onClick={handleBackButton}>Back</button>
                    }
                </div>
                <div className="headerColumnRightC">
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
                <div className="bodyColumnLeftC">
                    {/* You can add any content you want for the left column */}
                    <div className="textbody">Actions</div>
                    <div className="underline"></div>
                    <div className='create-container'>
                        <button className="eventButton" onClick={handleEventButtonClick}>Event</button>
                        <button className="eventButton" onClick={handleShareButtonClick}>Share</button>
                        {/* <button className="calendarButton" onClick={addCalendar}>Calendar</button> */}
                    </div>

                    {/* <div className="textbody2">My Calendars</div>
            <div className="underline"></div>
            {calendars.map((calendar, index) => (
              <div key={index} className='textCalendars'>
                <input type="checkbox" className="calendar-checkbox" style={{ backgroundColor: checkboxColors[index] }} />
                {editMode[index] ? (
                  <div>
                    <input type="text" value={calendar} onChange={(e) => {
                      const updatedCalendars = [...calendars];
                      updatedCalendars[index] = e.target.value;
                      setCalendars(updatedCalendars);
                    }} />
                    <input type="color" value={checkboxColors[index]} onChange={(e) => handleColorChange(index, e.target.value)} style={{ width: '20px', height: '20px', border: 'none', padding: '0', marginLeft: '10px' }} />
                    <img src={deleteIcon} alt="Delete" className="deleteButton" onClick={() => deleteCalendar(index)} />
                  </div>
                ) : (
                  <span>{calendar}</span>
                )}
                <img src={editIcon} alt="Edit" className="editButton" onClick={() => toggleEditMode(index)} />
              </div>
            ))} */}
                </div>
                <div className="bodyColumnRightC">
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

export default CalendarView;
