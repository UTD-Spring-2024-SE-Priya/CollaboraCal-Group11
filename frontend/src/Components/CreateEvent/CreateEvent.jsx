import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './CreateEvent.css';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import * as req from '../../Requests';

const CreateEvent = () => {
    const [title, setTitle] = useState(""); // State to track event title input value
    const [date, setDate] = useState(new Date()); // State to track event date input value
    const [time, setTime] = useState(""); // State to track event time input value
    const [description, setDescription] = useState(""); // State to track event description input value
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        if (location.state == null) {
            navigate("/home")
            return;
        }
        if (location.state.id == null) {
            navigate("/home")
            return;
        }
    }, [])

    const onEventCreatedResponse = async (response) => {

        if (response.status == 200) {
            navigate("/calendar", { state: { id: location.state.id } });
        }
    }

    const handleSubmit = () => {
        // Check if title, date, and time are empty
        if (!title.trim() || !date || !time.trim()) {
            setError("Please fill out all fields: Title, Date, and Time.");
            return;
        }

        // Check if date and time are valid
        if (!isValidDate(date) || !isValidTime(time)) {
            setError("Please enter a valid date and time.");
            return;
        }

        /*
    [JsonProperty("name")] string? Name,
    [JsonProperty("description")] string? Description,
    [JsonProperty("location")] string? Location,
    [JsonProperty("start")] DateTime StartTime,
    [JsonProperty("end")] DateTime EndTime,
    [JsonProperty("calendarID")] int CalendarID
        */

        let headers = req.createAuthHeaders()

        let mutableDate = new Date(date)
        let pTime = parseTime(time)
        mutableDate.setHours(pTime.getHours())
        mutableDate.setMinutes(pTime.getMinutes())

        let completeDate = mutableDate.toUTCString() //Date.parse(date.trim() + " " + time.trim());

        let body = JSON.stringify({
            name: title.trim(),
            start: completeDate,
            end: completeDate,
            location: "",
            calendarID: location.state.id,
            description: description,
        });

        req.postRequest("/newevent", headers, body)
            .then(onEventCreatedResponse)
            .catch(r => console.log(r));

        // If all validations pass, navigate to home with all information
        //console.log("Submitting event:", { title, date, time, description }); // Debugging statement


    };

    const isValidDate = (date) => {
        return true; // added DatePicker so should always be valid?
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

    function parseTime(t) {
        var time = t.match(/(\d+)(:(\d\d))?\s*([Pp]?)/i);
        var d = new Date();

        var hours = parseInt(time[1], 10);
        if (hours == 12 && !time[4]) {
            hours = 0;
        }
        else {
            hours += (hours < 12 && time[4]) ? 12 : 0;
        }

        d.setHours(hours)
        d.setMinutes(parseInt(time[3], 10) || 0);
        d.setSeconds(0, 0);
        return d;
    }

    const formatDate = (date) => {
        return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
    }

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
                        dateFormat="MM/dd/yyyy"
                        value={formatDate(date)}
                        onChange={(e) => { setDate(e); console.log(e) }}
                        className="input-date">
                    </DatePicker>

                    {/* <input
                        type="text"
                        placeholder="MM/DD/YYYY"
                        value={date}
                        onChange={(e) => setDate(e.target.value)}
                        className="input"
                    /> */}


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
