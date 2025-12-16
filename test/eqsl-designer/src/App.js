import { useState, useRef, useEffect } from "react";
import { Stage, Layer, Text, Image as KonvaImage } from "react-konva";

function useImage(url) {
  const [image, setImage] = useState(null);
  useEffect(() => {
    const img = new window.Image();
    img.src = url;
    img.onload = () => setImage(img);
  }, [url]);
  return image;
}

export default function App() {
  const stageRef = useRef(null);
  const [vars, setVars] = useState({
    callsign: "KJ5LVN",
    operatorName: "David",
    qth: "Sapulpa, Oklahoma",
    grid: "EM25",
    ituZone: "7",
    cqZone: "4",
    dateUTC: "2025-12-16",
    timeUTC: "02:15",
    band: "70cm",
    mode: "DMR",
    rst: "59",
    contactName: "W1AW"            // NEW: Contact name/callsign
  });

  // Load local background image from public folder
  const bg = useImage(process.env.PUBLIC_URL + "/FlagOklahoma.png");

  const exportPNG = () => {
    const uri = stageRef.current.toDataURL({ pixelRatio: 2 });
    const a = document.createElement("a");
    a.href = uri;
    a.download = `eQSL_${vars.callsign}.png`;
    a.click();
  };

  const width = 1200, height = 800;

  return (
    <div style={{ display: "grid", gridTemplateColumns: "1fr 320px", gap: 16 }}>
      <div>
        <Stage width={width} height={height} ref={stageRef}>
          <Layer>
            {/* Background image */}
            {bg && <KonvaImage image={bg} x={0} y={0} width={width} height={height} />}

            {/* Callsign */}
            <Text
              text={vars.callsign}
              x={40} y={40}
              fontSize={96}
              fontFamily="Orbitron"
              fill="#19f5ff"
              shadowColor="#19f5ff"
              shadowBlur={20}
              draggable
            />

            {/* Operator + QTH */}
            <Text
              text={`${vars.operatorName} • ${vars.qth}`}
              x={40} y={150}
              fontSize={28}
              fontFamily="Inter"
              fill="#031d3aff"
              draggable
            />

            {/* Grid Square (below QTH) */}
            <Text
              text={`Grid ${vars.grid}`}
              x={40} y={190}
              fontSize={26}
              fontFamily="Inter"
              fill="#031d3aff"
              draggable
            />

           
           {/* QSO Details */}
        <Text
           text={`QSO: ${vars.dateUTC} ${vars.timeUTC} UTC • ${vars.band} • ${vars.mode} • RST ${vars.rst}`}
           x={0} y={710}              // near bottom of 800px canvas
             width={1200}               // full canvas width
             align="center"             // center horizontally
            fontSize={24}
            fontFamily="Inter"
            fill="#00030aff"
            draggable
        />


            {/* Zones */}
            <Text
              text={`ITU ${vars.ituZone} • CQ ${vars.cqZone}`}
              x={40} y={310}
              fontSize={22}
              fontFamily="Inter"
              fill="#000308ff"
              draggable
            />

            {/* Contact Name */}
            <Text
              text={`Contact: ${vars.contactName}`}
              x={40} y={350}
              fontSize={26}
              fontFamily="Inter"
              fill="#031d3aff"
              draggable
            />
          </Layer>
        </Stage>

        {/* Export Button */}
        <div style={{ marginTop: 12 }}>
          <button onClick={exportPNG}>Export PNG</button>
        </div>
      </div>

      {/* Variables Panel */}
      <div>
        <h3>Variables</h3>
        {Object.entries(vars).map(([k, v]) => (
          <div key={k} style={{ marginBottom: 8 }}>
            <label style={{ display: "block", fontSize: 12 }}>{k}</label>
            <input
              value={v}
              onChange={(e) => setVars({ ...vars, [k]: e.target.value })}
              style={{ width: "100%" }}
            />
          </div>
        ))}
      </div>
    </div>
  );
}
