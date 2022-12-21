import React, {useState} from "react"
import 'bootstrap/dist/css/bootstrap.min.css';
import {useAuth} from "../../hook/useAuth";
import {Navigate, useNavigate} from "react-router-dom";
import "../common/css/loader.css"

function Login(){
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string>('');
    const [displayBtn, setDisplayBtn] = useState(false);
    const [loaderHide, setLoaderHide] = useState("hide");
    const auth = useAuth();
    
    const navigate = useNavigate();
    
    if (auth?.isAdminAuth()){
        return <Navigate to='/'/>
    }

    const onLoginClick = async (e: any) => {
        e.preventDefault()
        if (!(username && password)) {
            setError('заполните поля для входа');
            return;
        }

        const details:any = {
            'username': username,
            'password': password,
            'grant_type': 'password'
        };

        const formBody = [];
        for (let property in details) {
            const encodedKey = encodeURIComponent(property);
            const encodedValue = encodeURIComponent(details[property]);
            formBody.push(encodedKey + "=" + encodedValue);
        }

        setDisplayBtn(true);
        setLoaderHide("");
        const authResponse = await fetch(`${process.env.REACT_APP_IDENTITY_URL}/connect/token`, {
            method: 'POST',
            body: formBody.join("&"),
            mode: 'cors',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
        });
        setLoaderHide("hide");
        setDisplayBtn(false);

        if(!(authResponse.ok)){
            setError('неверный логин и/или пароль');
            return;
        }

        const token = (await authResponse.json())['access_token']

        if(!token){
            setError('ошибка');
            return;
        }

        if(!auth?.isAdmin(token)){
            setError('у вас нет прав администратора');
            return;
        }

        auth?.login(token);
        navigate("/", {replace: true});
    }

    return (
        <div className='mt-5 d-flex justify-content-center align-items-center'>
            <div className='align-items-center' style={{width:'18%'}}>
                <form>
                    <h1 className='text-center mt-5'>Вход</h1>
                    <input id='email' className='form-control mt-2' type='text' placeholder='Логин' value={username}
                           onInput={e => setUsername(e.currentTarget.value)}/>
                    <input id='password' className='form-control mt-2' type='password' placeholder='Пароль'
                           onInput={e => setPassword(e.currentTarget.value)}/>
                    <div className={'justify-content-center d-flex mt-2'}>
                        <button onClick={onLoginClick} style={{display: `${displayBtn ? "none" : "block"}`}}
                                className='btn btn btn-outline-dark justify-content-center w-100'>
                            Войти
                        </button>
                        <div className={`loader ${loaderHide}`}/>
                    </div>
                    {error && <span className={'text-danger d-block text-center'}>{error}</span>}
                </form>
            </div>
        </div>
    )
}

export default Login