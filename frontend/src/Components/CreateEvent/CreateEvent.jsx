import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './CreateEvent.css';
import DatePicker from "react-datepicker";

const CreateEvent = () =>  {
    const [title, setTitle] = useState(""); // State to track event title input value
    const [date, setDate] = useState(""); // State to track event date input value
    const [time, setTime] = useState(""); // State to track event time input value
    const [description, setDescription] = useState(""); // State to track event description input value
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleSubmit = () => {
        // Check if title, date, and time are empty
        if (!title.trim() || !date.trim() || !time.trim()) {
            setError("Please fill out all fields: Title, Date, and Time.");
            return;
        }

        // Check if date and time are valid
        if (!isValidDate(date) || !isValidTime(time)) {
            setError("Please enter a valid date and time.");
            return;
        }

        // If all validations pass, navigate to home with all information
        console.log("Submitting event:", { title, date, time, description }); // Debugging statement
        navigate("/home", {
            state: { title, date, time, description }
        });
    };

    const isValidDate = (date) => {
        // Regular expression to validate date format (MM/DD/YYYY)
        const dateFormat = /^\d{2}\/\d{2}\/\d{4}$/;

        if (!date.match(dateFormat)) {
            return false; // Date format is invalid
        }

        const parsedDate = new Date(date);
        if (isNaN(parsedDate.getTime())) {
            return false; // Date is invalid
        }

        return true; // Date is valid
    };

    const isValidTime = (time) => {
        // Regular expression to validate time format (HH:MM AM/PM)
        const timeFormat = /^(0?[1-9]|1[012])(:[0-5]\d) [APap][mM]$/;

        if (!time.match(timeFormat)) {
            return false; // Time format is invalid
        }

        return true; // Time is valid
    };

    return(
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
                    <input 
                        type="text" 
                        placeholder="MM/DD/YYYY" 
                        value={date} 
                        onChange={(e) => setDate(e.target.value)} 
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
