import { useState } from 'react'
import { useAuth } from './AuthContext.jsx'

function LogInPage() {
    const [form, setForm] = useState({
        email: '',
        password: '',
    });
    const [error, setError] = useState("")

    const { logIn } = useAuth();

    async function handleSubmit(e) {
        e.preventDefault();

        try {
            await logIn(form.email, form.password);
        }
        catch (error) {
            console.error('Error during login:', error);
            setError("Incorrect email or password!");
        }
    }

    function handleChange(e) {
        const { name, value } = e.target;
        setForm(prevLogin => ({
            ...prevLogin,
            [name]: value
        }));
    }

    return (<div>
            {error != "" &&
            <>
                <p style={{ color: "red" }}>{error}</p>
            </>
            }

            <h2>Log In</h2>
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
                </div>
                <button type="submit">Log In</button>
            </form>
        </div>)
}

export default LogInPage;