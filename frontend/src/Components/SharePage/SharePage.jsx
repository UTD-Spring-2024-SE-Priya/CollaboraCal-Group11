import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import '../CreateEvent/CreateEvent.css';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import * as req from '../../Requests';

const SharePage = () => {
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

        let headers = req.createAuthHeaders()

        let body = JSON.stringify({
            to: title,
            calendarID: location.state.id
        });

        req.postRequest("/share", headers, body)
            .then(onEventCreatedResponse)
            .catch(r => console.log(r));

        // If all validations pass, navigate to home with all information
        //console.log("Submitting event:", { title, date, time, description }); // Debugging statement


    };

    const isValidDate = (date) => {
        return true; // added DatePicker so should always be valid?
    };

    const isValidTime = (time) => {
        // Regular expression to validate time format (HH:MM AM/PM)
        const timeFormat = /^(0?[1-9]|1[012])(:[0-5]\d) [APap][mM]$/;

        if (!time.match(timeFormat)) {
            return false; // Time format is invalid
        }

        return true; // Time is valid
    };



    const formatDate = (date) => {
        return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
    }

    return (
        <div className="container">
            <div className="header">
                <div className="text">Share Calendar</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input-container">
                    <div className="info">To: </div>
                    <input
                        type="text"
                        placeholder="Enter share email"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        className="input"
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

export default SharePage;
