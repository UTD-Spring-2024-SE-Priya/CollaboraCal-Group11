import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './CreateCalendar.css';

const CreateCalendar = () =>  {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleSubmit = () => {
        if (!name.trim()) {
            setError("Please enter a calendar name.");
            return;
        }
        // If name field is filled, you can proceed with navigation or form submission
        // For demonstration, let's just navigate to another page
        navigate("/home"); // Replace "/some-route" with the desired route
    };

    return(
        <div className="container">
            <div className="header">
                <div className="text">Create Calendar</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input-container">
                    <div className="info">Name</div>
                    <input 
                        type="text" 
                        placeholder="Enter calendar name" 
                        value={name} 
                        onChange={(e) => setName(e.target.value)} 
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

export default CreateCalendar;
