import React, { useState } from 'react';
import './LoginSignUp.css';
import { useNavigate } from 'react-router-dom'; // Import useNavigate from react-router-dom
import user_icon from '../Assets/person.png';
import email_icon from '../Assets/email.png';
import password_icon from '../Assets/password.png';

const LoginSignUp = () => {
    const [action, setAction] = useState("Login");
    const [name, setName] = useState(""); // State to track name input value
    const [email, setEmail] = useState(""); // State to track email input value
    const [password, setPassword] = useState(""); // State to track password input value
    const [error, setError] = useState(""); // State to track error message
    const navigate = useNavigate(); // Initialize useNavigate

    const handleToggleClick = () => {
        setAction(prevAction => prevAction === "Login" ? "Sign Up" : "Login");
        // Reset error message when toggling between Login and Sign Up
        setError("");
    };

    const handleSubmitClick = () => {
        if (action === "Login" ? !email || !password : !name || !email || !password) {
            // Set error message if fields are not filled
            setError("Please fill in all fields.");
        } else {
            // Redirect to the "/home" route
            navigate("/home");
        }
    };

    return (
        <div className='container'>
            <div className="header">
                <div className="text">{action}</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                {action === "Login" ? null :
                    <div className="input">
                        <img src={user_icon} alt="" />
                        <input type="text" placeholder='Name' value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                }
                <div className="input">
                    <img src={email_icon} alt="" />
                    <input type="email" placeholder='Email ID' value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className="input">
                    <img src={password_icon} alt="" />
                    <input type="password" placeholder='Password' value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
            </div>
            <div className="forgot-password">Forgot Password? <span>Click Here!</span></div>
            {error && <div className="error-message errormessage">{error}</div>}
            <div className="submit-container">
                <div className="submit" onClick={handleToggleClick}>{action === "Login" ? "Sign Up" : "Login"}</div>
                <div className={`submit ${error ? 'error' : ''}`} onClick={handleSubmitClick}>Submit</div>
            </div>
            <div className="website-name">
                CollaboraCal
            </div>
        </div>
    );
};

export default LoginSignUp;
