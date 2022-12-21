import {CSSProperties, FC} from "react";

interface IconProps {
    name: string;
    width?: number;
    height?: number;
    styles?: CSSProperties;
}

const Icon: FC<IconProps> = (props) => {
    const {
        name,
        width,
        height
    } = props;

    return (
        <svg
            version='1.1'
            xmlns='http://www.w3.org/2000/svg'
            style={{
                width: `${width}px`,
                height: `${height}px`,
                ...props.styles
            }}
        >
            <use xlinkHref={`/sprite.svg#${name}`}/>
        </svg>
    );
}
export default Icon