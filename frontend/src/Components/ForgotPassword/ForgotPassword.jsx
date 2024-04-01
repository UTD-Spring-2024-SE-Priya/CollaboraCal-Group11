import React, { useState } from 'react';
import './ForgotPassword.css';
import { useNavigate } from 'react-router-dom';
import email_icon from '../Assets/email.png';
import password_icon from '../Assets/password.png';

const ForgotPassword = () => {
    const [email, setEmail] = useState(""); // State to track email input value
    const [password, setPassword] = useState(""); // State to track password input value
    const [confirmPassword, setConfirmPassword] = useState(""); // State to track confirm password input value
    const navigate = useNavigate(); // Initialize useNavigate
    const [error, setError] = useState(""); // State to track error message

    const validateEmail = (email) => {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    };

    const validatePassword = (password) => {
        // Password should be at least 8 characters long and contain at least 1 uppercase, 1 lowercase, and 1 number
        const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
        return passwordRegex.test(password);
    };

    const handleSubmitClick = () => {
        if (!email || !password || !confirmPassword) {
            setError("Please fill in all fields.");
            return;
        }

        if (!validateEmail(email)) {
            setError("Please enter a valid email.");
            return;
        }

        if (password.length < 8 || !validatePassword(password)) {
            setError("Password should be at least 8 characters long with at least 1 uppercase, 1 lowercase, and 1 number.");
            return;
        }

        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        // Reset error message if all validations pass
        setError("");

        // Your logic for handling the submission goes here
        // Redirect or perform any other action
        navigate("/home"); // Example redirect to success page
    };

    return (
        <div className="container"> 
            <div className="header">
                <div className="text">Reset Password</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input">
                    <img src={email_icon} alt="" />
                    <input type="email" placeholder='Email ID' value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className="input">
                    <img src={password_icon} alt="" />
                    <input type="password" placeholder='New Password' value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className="input">
                    <img src={password_icon} alt="" />
                    <input type="password" placeholder='Confirm New Password' value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
                </div>
            </div>
            {error && <div className="error-message errormessage">{error}</div>}
            <div className="submit-container">
                <div className={`submit ${error ? 'error' : ''}`} onClick={handleSubmitClick}>Submit</div>
            </div>
        </div>
    );
};

export default ForgotPassword;
