// CreateEvent.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './CreateEvent.css';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const CreateEvent = ({ addEvent }) => {
  const [title, setTitle] = useState("");
  const [date, setDate] = useState(null);
  const [time, setTime] = useState("");
  const [description, setDescription] = useState("");
  const [error, setError] = useState("");
  const [events, setEvents] = useState([]);
  const navigate = useNavigate();

  const handleSubmit = () => {
    if (!title.trim() || !date || !time.trim()) {
      setError("Please fill out all fields: Title, Date, and Time.");
      return;
    }

    if (!isValidTime(time)) {
      setError("Please enter a valid time.");
      return;
    }

    const newEvent = {
      title: title,
      date: date,
      time: time,
      description: description
    };

    
    setEvents([...events, newEvent]);

    addEvent(newEvent);

    navigate("/home"); // Navigate to home
  };

  console.log("Submitting event:", { title, date, time, description }); // Debugging statement
  navigate("/home", {
      state: { title, date, time, description }
  });

  const isValidTime = (time) => {
    const timeFormat = /^(0?[1-9]|1[012])(:[0-5]\d) [APap][mM]$/;

    if (!time.match(timeFormat)) {
      return false;
    }

    return true;
  };

  return (
    <div className="container">
      <div className="header">
        <div className="text">Create Event</div>
        <div className="underline"></div>
      </div>
      <div className="inputs">
        <div className="input-container">
          <div className="info">Title</div>
          <input
            type="text"
            placeholder="Enter event title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="input"
          />
        </div>
        <div className="input-container">
          <div className="info">Date</div>
          <DatePicker
            selected={date}
            onChange={(date) => setDate(date)}
            placeholderText="Select date"
            className="input"
          />
        </div>
        <div className="input-container">
          <div className="info">Time</div>
          <input
            type="text"
            placeholder="12:00 AM"
            value={time}
            onChange={(e) => setTime(e.target.value)}
            className="input"
          />
        </div>
        <div className="input-container">
          <div className="info">Description</div>
          <input
            type="text"
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className="description-input"
          />
        </div>
      </div>
      {error && <div className="errormessage">{error}</div>}
      <div className="submit-container">
        <div className="submit" onClick={handleSubmit}>Submit</div>
      
      </div>
    </div>
  );
};

export default CreateEvent;