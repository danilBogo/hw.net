import 'bootstrap/dist/css/bootstrap.min.css';
import {useAuth} from "../../hook/useAuth";
import {useNavigate} from "react-router-dom";

function Logout(){
    const auth = useAuth();
    const navigate = useNavigate();
    
    const onlogoutOnClick = async (e:any) => {
        auth?.logout();
        return navigate("/");
    }

    return <button onClick={onlogoutOnClick} className='ms-3 btn btn-outline-dark'>Выйти</button>
}

export default Logout