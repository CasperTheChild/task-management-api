import MainPage from './components/MainPage.jsx'
import LogInPage from './auth/LogInPage.jsx'
import { useAuth } from './auth/AuthContext.jsx'
import { Register } from './auth/Register.jsx'

function App() {
    const { token } = useAuth();

    return (
        <div
            className="container-fluid min-vh-100 bg-dark text-light btn-dark p-2"
        >

        {token == null && <div>
        <LogInPage />
        <Register />
        </div>}
        { token && <MainPage /> }
    
        </div>
    )
}

export default App
