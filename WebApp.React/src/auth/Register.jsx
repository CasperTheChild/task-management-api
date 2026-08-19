import { useState } from 'react';
import { checkEmail, checkPassword } from '../utils/validator.jsx'
import { RegisterApi } from '../api/AuthApi.js'

export function Register() {

    const [error, setError] = useState("");

    const [form, setForm] = useState({
        email: '',
        password: '',
        confirmPassword: '',
    });

    function handleChange(e) {
        const { name, value } = e.target;
        setForm(prevForm => ({
            ...prevForm,
            [name]: value
        }));
    }

    function handleSubmit(e) {

        if (!checkEmail(form.email)) {
            e.preventDefault();
            setError("Incorrect email format!");
            return;
        }

        if (!checkPassword(form.password)) {
            e.preventDefault();
            setError("Incorrect password format!");
            return;
        }

        if (form.password != form.confirmPassword) {
            e.preventDefault();
            setError("Passwords are not equal!");
            return;
        }

        RegisterApi(form.email, form.password, form.confirmPassword);

        setError("");
    }

    return (
        <div>
            {error != "" &&
                <>
                    <p style={{ color: "red" }}>{error}</p>
                </>
            }

            <h2>Register</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="email">Email:</label>
                    <input
                        id="email"
                        type="email"
                        name="email"
                        value={form.email}
                        onChange={handleChange} />

                    <label htmlFor="password">Password:</label>
                    <input
                        id="password"
                        type="password"
                        name="password"
                        value={form.password}
                        onChange={handleChange} />

                    <label htmlFor="confirmPassword">Confirm Password:</label>
                    <input
                        id="confirmPassword"
                        type="password"
                        name="confirmPassword"
                        value={form.confirmPassword}
                        onChange={handleChange} />
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    )
}